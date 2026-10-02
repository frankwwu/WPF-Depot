using Kanban.Models;
using Kanban.ViewModels;

namespace Kanban.Tests;

public class BoardViewModelTests
{
    private static readonly User Ada = new(1, "Ada", "Lovelace");
    private static readonly User Alan = new(2, "Alan", "Turing");

    private readonly FakeRepository _repository = new();
    private readonly FakeDialogs _dialogs = new();

    private BoardViewModel CreateBoard(params WorkItemSnapshot[] items) =>
        new(new BoardSnapshot { Users = [Ada, Alan], Items = items }, _repository, _dialogs);

    private static WorkItemSnapshot Item(int id, string title, WorkItemStatus status, int? assignee = null) =>
        new(id, title, status, assignee);

    private static ColumnViewModel Column(BoardViewModel board, WorkItemStatus status) =>
        board.Columns.Single(c => c.Status == status);

    private static string[] Titles(BoardViewModel board, WorkItemStatus status) =>
        [.. Column(board, status).Items.Select(c => c.Item.Title)];

    private static CardViewModel Card(BoardViewModel board, string title) =>
        board.Columns.SelectMany(c => c.Items).Single(c => c.Item.Title == title);

    // ---- loading -------------------------------------------------------------------------

    [Fact]
    public void Creates_one_column_per_status_even_when_the_board_is_empty()
    {
        var board = CreateBoard();

        Assert.Equal(
            new[] { WorkItemStatus.New, WorkItemStatus.InProgress, WorkItemStatus.Done },
            board.Columns.Select(c => c.Status));
        Assert.All(board.Columns, column => Assert.Empty(column.Items));
    }

    [Fact]
    public void Distributes_items_into_their_columns_in_snapshot_order()
    {
        var board = CreateBoard(
            Item(1, "a", WorkItemStatus.Done),
            Item(2, "b", WorkItemStatus.New),
            Item(3, "c", WorkItemStatus.Done),
            Item(4, "d", WorkItemStatus.InProgress));

        Assert.Equal(new[] { "b" }, Titles(board, WorkItemStatus.New));
        Assert.Equal(new[] { "d" }, Titles(board, WorkItemStatus.InProgress));
        Assert.Equal(new[] { "a", "c" }, Titles(board, WorkItemStatus.Done));
        Assert.Equal(2, Column(board, WorkItemStatus.Done).Count);
    }

    [Fact]
    public void Resolves_assignees_and_treats_unknown_ids_as_unassigned()
    {
        var board = CreateBoard(
            Item(1, "known", WorkItemStatus.New, assignee: 2),
            Item(2, "unknown", WorkItemStatus.New, assignee: 99),
            Item(3, "none", WorkItemStatus.New));

        Assert.Equal(Alan, Card(board, "known").Item.Assignee);
        Assert.Null(Card(board, "unknown").Item.Assignee);
        Assert.Equal("Unassigned", Card(board, "none").AssigneeName);
    }

    // ---- adding --------------------------------------------------------------------------

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Add_is_disabled_for_a_blank_title(string title)
    {
        var board = CreateBoard();

        board.NewItemTitle = title;

        Assert.False(board.AddItemCommand.CanExecute(null));
    }

    [Fact]
    public void Add_puts_a_trimmed_item_at_the_top_of_New_with_the_next_id_and_clears_the_input()
    {
        var board = CreateBoard(Item(7, "existing", WorkItemStatus.New));

        board.NewItemTitle = "  brand new  ";
        Assert.True(board.AddItemCommand.CanExecute(null));
        board.AddItemCommand.Execute(null);

        Assert.Equal(new[] { "brand new", "existing" }, Titles(board, WorkItemStatus.New));
        Assert.Equal(8, Column(board, WorkItemStatus.New).Items[0].Item.Id);
        Assert.Equal(string.Empty, board.NewItemTitle);
        Assert.False(board.AddItemCommand.CanExecute(null));
        Assert.Equal(1, _repository.SaveCount);
    }

    [Fact]
    public void Add_raises_CanExecuteChanged_as_the_title_changes()
    {
        var board = CreateBoard();
        var raised = 0;
        board.AddItemCommand.CanExecuteChanged += (_, _) => raised++;

        board.NewItemTitle = "x";
        board.NewItemTitle = "x"; // unchanged: no notification

        Assert.Equal(1, raised);
    }

    // ---- moving --------------------------------------------------------------------------

    [Fact]
    public void Moving_to_another_column_inserts_at_the_slot_and_updates_the_status()
    {
        var board = CreateBoard(
            Item(1, "a", WorkItemStatus.New),
            Item(2, "x", WorkItemStatus.Done),
            Item(3, "y", WorkItemStatus.Done));

        board.MoveCard(Card(board, "a"), WorkItemStatus.Done, index: 1);

        Assert.Empty(Titles(board, WorkItemStatus.New));
        Assert.Equal(new[] { "x", "a", "y" }, Titles(board, WorkItemStatus.Done));
        Assert.Equal(WorkItemStatus.Done, Card(board, "a").Item.Status);
        Assert.Equal(1, _repository.SaveCount);
    }

    [Fact]
    public void Moving_without_an_index_appends_to_the_bottom()
    {
        var board = CreateBoard(
            Item(1, "a", WorkItemStatus.New),
            Item(2, "x", WorkItemStatus.InProgress));

        board.MoveCard(Card(board, "a"), WorkItemStatus.InProgress);

        Assert.Equal(new[] { "x", "a" }, Titles(board, WorkItemStatus.InProgress));
    }

    [Theory]
    [InlineData("a", 3, "b,c,a")] // to the very bottom
    [InlineData("a", 2, "b,a,c")] // between b and c
    [InlineData("c", 0, "c,a,b")] // to the very top
    [InlineData("c", 1, "a,c,b")]
    [InlineData("b", 0, "b,a,c")]
    public void Reordering_within_a_column_uses_the_slot_from_before_the_move(string moved, int slot, string expected)
    {
        var board = CreateBoard(
            Item(1, "a", WorkItemStatus.New),
            Item(2, "b", WorkItemStatus.New),
            Item(3, "c", WorkItemStatus.New));

        board.MoveCard(Card(board, moved), WorkItemStatus.New, slot);

        Assert.Equal(expected.Split(','), Titles(board, WorkItemStatus.New));
    }

    [Theory]
    [InlineData("b", 1)] // dropped just above itself
    [InlineData("b", 2)] // dropped just below itself
    public void Dropping_a_card_back_where_it_was_changes_nothing_and_does_not_save(string moved, int slot)
    {
        var board = CreateBoard(
            Item(1, "a", WorkItemStatus.New),
            Item(2, "b", WorkItemStatus.New),
            Item(3, "c", WorkItemStatus.New));

        board.MoveCard(Card(board, moved), WorkItemStatus.New, slot);

        Assert.Equal(new[] { "a", "b", "c" }, Titles(board, WorkItemStatus.New));
        Assert.Equal(0, _repository.SaveCount);
    }

    [Fact]
    public void An_out_of_range_slot_is_clamped()
    {
        var board = CreateBoard(
            Item(1, "a", WorkItemStatus.New),
            Item(2, "x", WorkItemStatus.Done));

        board.MoveCard(Card(board, "a"), WorkItemStatus.Done, index: 500);

        Assert.Equal(new[] { "x", "a" }, Titles(board, WorkItemStatus.Done));
    }

    [Fact]
    public void MoveTo_command_is_disabled_for_the_current_column_and_moves_to_the_bottom_otherwise()
    {
        var board = CreateBoard(
            Item(1, "a", WorkItemStatus.New),
            Item(2, "x", WorkItemStatus.Done));
        var card = Card(board, "a");

        Assert.False(card.MoveToCommand.CanExecute(WorkItemStatus.New));
        Assert.True(card.MoveToCommand.CanExecute(WorkItemStatus.Done));

        card.MoveToCommand.Execute(WorkItemStatus.Done);

        Assert.Equal(new[] { "x", "a" }, Titles(board, WorkItemStatus.Done));
        Assert.False(card.MoveToCommand.CanExecute(WorkItemStatus.Done));
        Assert.True(card.MoveToCommand.CanExecute(WorkItemStatus.New));
    }

    [Fact]
    public void MoveTo_command_raises_CanExecuteChanged_when_the_card_changes_column()
    {
        var board = CreateBoard(Item(1, "a", WorkItemStatus.New));
        var card = Card(board, "a");
        var raised = 0;
        card.MoveToCommand.CanExecuteChanged += (_, _) => raised++;

        board.MoveCard(card, WorkItemStatus.Done);

        Assert.Equal(1, raised);
    }

    [Fact]
    public void Moving_a_card_that_is_not_on_the_board_does_nothing()
    {
        var board = CreateBoard(Item(1, "a", WorkItemStatus.New));
        var card = Card(board, "a");
        _dialogs.ConfirmResult = true;
        board.Delete(card);
        var saves = _repository.SaveCount;

        board.MoveCard(card, WorkItemStatus.Done);

        Assert.Equal(saves, _repository.SaveCount);
        Assert.All(board.Columns, column => Assert.Empty(column.Items));
    }

    // ---- editing / deleting / assigning ---------------------------------------------------

    [Fact]
    public void Edit_changes_the_title_with_surrounding_whitespace_trimmed()
    {
        var board = CreateBoard(Item(1, "old", WorkItemStatus.New));
        _dialogs.PromptResult = "  new title ";

        Card(board, "old").EditCommand.Execute(null);

        Assert.Equal(new[] { "new title" }, Titles(board, WorkItemStatus.New));
        Assert.Equal(1, _repository.SaveCount);
    }

    [Theory]
    [InlineData(null)]   // cancelled
    [InlineData("   ")]  // blank
    [InlineData("old")]  // unchanged
    public void Edit_is_ignored_when_cancelled_blank_or_unchanged(string? promptResult)
    {
        var board = CreateBoard(Item(1, "old", WorkItemStatus.New));
        _dialogs.PromptResult = promptResult;

        Card(board, "old").EditCommand.Execute(null);

        Assert.Equal(new[] { "old" }, Titles(board, WorkItemStatus.New));
        Assert.Equal(0, _repository.SaveCount);
    }

    [Fact]
    public void Delete_removes_the_card_only_after_confirmation()
    {
        var board = CreateBoard(Item(1, "a", WorkItemStatus.New), Item(2, "b", WorkItemStatus.New));

        _dialogs.ConfirmResult = false;
        Card(board, "a").DeleteCommand.Execute(null);
        Assert.Equal(new[] { "a", "b" }, Titles(board, WorkItemStatus.New));
        Assert.Equal(0, _repository.SaveCount);

        _dialogs.ConfirmResult = true;
        Card(board, "a").DeleteCommand.Execute(null);
        Assert.Equal(new[] { "b" }, Titles(board, WorkItemStatus.New));
        Assert.Equal(1, _repository.SaveCount);
    }

    [Fact]
    public void Assign_and_Unassign_commands_track_the_current_assignee()
    {
        var board = CreateBoard(Item(1, "a", WorkItemStatus.New, assignee: 1));
        var card = Card(board, "a");

        Assert.False(card.AssignCommand.CanExecute(Ada));   // already assigned to Ada
        Assert.True(card.AssignCommand.CanExecute(Alan));
        Assert.True(card.UnassignCommand.CanExecute(null));

        card.AssignCommand.Execute(Alan);
        Assert.Equal(Alan, card.Item.Assignee);
        Assert.Equal("Alan Turing", card.AssigneeName);

        card.UnassignCommand.Execute(null);
        Assert.Null(card.Item.Assignee);
        Assert.Equal("Unassigned", card.AssigneeName);
        Assert.False(card.UnassignCommand.CanExecute(null));
        Assert.Equal(2, _repository.SaveCount);
    }

    [Fact]
    public void Changing_the_assignee_notifies_the_view()
    {
        var board = CreateBoard(Item(1, "a", WorkItemStatus.New));
        var card = Card(board, "a");
        var changed = new List<string?>();
        card.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        board.Assign(card, Ada);

        Assert.Contains(nameof(CardViewModel.AssigneeName), changed);
    }

    // ---- persistence ---------------------------------------------------------------------

    [Fact]
    public void Saves_the_board_column_by_column_so_order_survives_a_reload()
    {
        var board = CreateBoard(
            Item(1, "a", WorkItemStatus.Done, assignee: 1),
            Item(2, "b", WorkItemStatus.New),
            Item(3, "c", WorkItemStatus.Done));

        board.MoveCard(Card(board, "c"), WorkItemStatus.Done, index: 0);

        var saved = _repository.Saved!;
        Assert.Equal(new[] { 2, 3, 1 }, saved.Items.Select(i => i.Id)); // New, then Done: c, a
        Assert.Equal(new int?[] { null, null, 1 }, saved.Items.Select(i => i.AssigneeId));
        Assert.Equal(new[] { Ada, Alan }, saved.Users);

        var reloaded = new BoardViewModel(saved, _repository, _dialogs);
        Assert.Equal(new[] { "c", "a" }, Titles(reloaded, WorkItemStatus.Done));
    }

    [Fact]
    public void Ids_of_new_items_never_collide_with_loaded_ones()
    {
        var board = CreateBoard(Item(41, "a", WorkItemStatus.Done));

        board.NewItemTitle = "b";
        board.AddItemCommand.Execute(null);

        Assert.Equal(42, Card(board, "b").Item.Id);
    }

    [Fact]
    public void A_failing_save_is_reported_once_and_does_not_throw()
    {
        var board = CreateBoard(Item(1, "a", WorkItemStatus.New));
        _repository.FailWith = new IOException("disk full");

        board.MoveCard(Card(board, "a"), WorkItemStatus.Done);
        board.MoveCard(Card(board, "a"), WorkItemStatus.InProgress);

        var error = Assert.Single(_dialogs.Errors);
        Assert.Contains("disk full", error);
    }

    [Fact]
    public void After_a_failed_save_recovers_the_next_failure_is_reported_again()
    {
        var board = CreateBoard(Item(1, "a", WorkItemStatus.New));
        var card = Card(board, "a");

        _repository.FailWith = new IOException("first");
        board.MoveCard(card, WorkItemStatus.Done);
        _repository.FailWith = null;
        board.MoveCard(card, WorkItemStatus.InProgress);
        _repository.FailWith = new IOException("second");
        board.MoveCard(card, WorkItemStatus.New);

        Assert.Equal(2, _dialogs.Errors.Count);
    }
}
