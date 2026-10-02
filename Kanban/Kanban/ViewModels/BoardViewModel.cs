using System.IO;
using Kanban.Models;
using Kanban.Mvvm;
using Kanban.Services;

namespace Kanban.ViewModels;

/// <summary>
/// The whole board: one column per <see cref="WorkItemStatus"/>, plus the operations on it.
/// Every mutation is saved through the <see cref="IBoardRepository"/> straight away.
/// </summary>
public sealed class BoardViewModel : ObservableObject
{
    private readonly IBoardRepository _repository;
    private readonly IDialogService _dialogs;
    private int _nextId;
    private bool _saveFailureReported;

    public BoardViewModel(BoardSnapshot snapshot, IBoardRepository repository, IDialogService dialogs)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(dialogs);

        _repository = repository;
        _dialogs = dialogs;

        Users = [.. snapshot.Users.DistinctBy(u => u.Id)];
        Columns = [.. Enum.GetValues<WorkItemStatus>().Select(status => new ColumnViewModel(status))];

        var usersById = Users.ToDictionary(u => u.Id);
        foreach (var data in snapshot.Items)
        {
            // An unknown status (e.g. from a hand-edited file) lands in the first column.
            var column = Columns.FirstOrDefault(c => c.Status == data.Status) ?? Columns[0];
            var assignee = data.AssigneeId is { } id && usersById.TryGetValue(id, out var user) ? user : null;

            column.Items.Add(new CardViewModel(new WorkItem(data.Id, data.Title, column.Status, assignee), this));
        }

        _nextId = snapshot.Items.Select(i => i.Id).DefaultIfEmpty(0).Max() + 1;

        AddItemCommand = new DelegateCommand(AddItemFromInput, () => !string.IsNullOrWhiteSpace(NewItemTitle));
    }

    public IReadOnlyList<ColumnViewModel> Columns { get; }

    public IReadOnlyList<User> Users { get; }

    /// <summary>The text of the "add work item" box.</summary>
    public string NewItemTitle
    {
        get;
        set
        {
            if (SetProperty(ref field, value))
            {
                AddItemCommand.RaiseCanExecuteChanged();
            }
        }
    } = string.Empty;

    /// <summary>Adds <see cref="NewItemTitle"/> to the top of the New column.</summary>
    public DelegateCommand AddItemCommand { get; }

    public void EditTitle(CardViewModel card)
    {
        var title = _dialogs.PromptForText("Edit work item", $"Title for #{card.Item.Id}", card.Item.Title)?.Trim();
        if (string.IsNullOrEmpty(title) || title == card.Item.Title)
        {
            return;
        }

        card.Item.Title = title;
        Persist();
    }

    public void Delete(CardViewModel card)
    {
        if (!_dialogs.Confirm("Delete work item", $"Delete #{card.Item.Id} \"{card.Item.Title}\"?"))
        {
            return;
        }

        if (ColumnFor(card.Item.Status).Items.Remove(card))
        {
            Persist();
        }
    }

    public void Assign(CardViewModel card, User? user)
    {
        if (card.Item.Assignee == user)
        {
            return;
        }

        card.Item.Assignee = user;
        Persist();
    }

    /// <summary>
    /// Moves <paramref name="card"/> into the column for <paramref name="target"/>.
    /// </summary>
    /// <param name="card">The card to move.</param>
    /// <param name="target">The destination column.</param>
    /// <param name="index">
    /// The slot to drop into, counted in the destination column <em>as it is before the move</em>
    /// (0 = above the first card, <c>Count</c> = below the last). <see langword="null"/> means the bottom.
    /// </param>
    public void MoveCard(CardViewModel card, WorkItemStatus target, int? index = null)
    {
        var source = ColumnFor(card.Item.Status);
        var destination = ColumnFor(target);

        var from = source.Items.IndexOf(card);
        if (from < 0)
        {
            return; // not on this board (e.g. already deleted)
        }

        var slot = Math.Clamp(index ?? destination.Items.Count, 0, destination.Items.Count);

        if (source == destination)
        {
            // Dropping below its own position: the slot shifts up by one once the card is lifted out.
            var to = slot > from ? slot - 1 : slot;
            if (to == from)
            {
                return;
            }

            source.Items.Move(from, to);
        }
        else
        {
            source.Items.RemoveAt(from);
            destination.Items.Insert(slot, card);
            card.Item.Status = target;
        }

        Persist();
    }

    private void AddItemFromInput()
    {
        var title = NewItemTitle.Trim();
        if (title.Length == 0)
        {
            return;
        }

        var card = new CardViewModel(new WorkItem(_nextId++, title), this);
        ColumnFor(WorkItemStatus.New).Items.Insert(0, card);

        NewItemTitle = string.Empty;
        Persist();
    }

    private ColumnViewModel ColumnFor(WorkItemStatus status) => Columns.First(c => c.Status == status);

    private BoardSnapshot CreateSnapshot() => new()
    {
        Users = Users,
        Items =
        [
            .. Columns
                .SelectMany(column => column.Items)
                .Select(card => new WorkItemSnapshot(
                    card.Item.Id,
                    card.Item.Title,
                    card.Item.Status,
                    card.Item.Assignee?.Id)),
        ],
    };

    private void Persist()
    {
        try
        {
            _repository.Save(CreateSnapshot());
            _saveFailureReported = false;
        }
        catch (Exception ex) when (ex is System.IO.IOException or UnauthorizedAccessException)
        {
            // Tell the user once, not on every subsequent change.
            if (!_saveFailureReported)
            {
                _saveFailureReported = true;
                _dialogs.ShowError($"Couldn't save the board: {ex.Message}");
            }
        }
    }
}
