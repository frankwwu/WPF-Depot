using Kanban.Models;
using Kanban.Mvvm;
using Kanban.Services;

namespace Kanban.Tests;

public class UserTests
{
    [Theory]
    [InlineData("Ada", "Lovelace", "AL")]
    [InlineData("  ada ", "lovelace", "AL")]
    [InlineData("Ada", "", "A")]
    [InlineData("", "", "")]
    public void Initials_are_the_upper_case_first_letters(string given, string surname, string expected)
    {
        Assert.Equal(expected, new User(1, given, surname).Initials);
    }

    [Fact]
    public void FullName_joins_the_names_without_stray_spaces()
    {
        Assert.Equal("Ada Lovelace", new User(1, "Ada", "Lovelace").FullName);
        Assert.Equal("Ada", new User(1, "Ada", "").FullName);
    }
}

public class WorkItemTests
{
    [Fact]
    public void Property_changes_are_announced_only_when_the_value_changes()
    {
        var item = new WorkItem(1, "a");
        var changed = new List<string?>();
        item.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        item.Title = "b";
        item.Title = "b";
        item.Status = WorkItemStatus.Done;
        item.Assignee = new User(1, "Ada", "Lovelace");

        Assert.Equal(new[] { "Title", "Status", "Assignee" }, changed);
    }
}

public class DelegateCommandTests
{
    [Fact]
    public void Execute_does_nothing_while_CanExecute_is_false()
    {
        var ran = false;
        var command = new DelegateCommand(() => ran = true, () => false);

        command.Execute(null);

        Assert.False(ran);
    }

    [Fact]
    public void RaiseCanExecuteChanged_notifies_subscribers()
    {
        var command = new DelegateCommand(() => { });
        var raised = 0;
        command.CanExecuteChanged += (_, _) => raised++;

        command.RaiseCanExecuteChanged();

        Assert.Equal(1, raised);
    }

    [Fact]
    public void Typed_command_rejects_a_parameter_of_the_wrong_type()
    {
        var received = new List<int>();
        var command = new DelegateCommand<int>(received.Add);

        Assert.False(command.CanExecute("nope"));
        Assert.False(command.CanExecute(null)); // null isn't an int
        command.Execute("nope");
        command.Execute(5);

        Assert.Equal(new[] { 5 }, received);
    }

    [Fact]
    public void Typed_command_accepts_null_for_reference_types()
    {
        string? received = "unset";
        var command = new DelegateCommand<string?>(value => received = value);

        Assert.True(command.CanExecute(null));
        command.Execute(null);

        Assert.Null(received);
    }
}

public class SampleDataTests
{
    [Fact]
    public void Creates_a_consistent_board_with_unique_ids()
    {
        var board = SampleData.Create(userCount: 3, itemCount: 9);

        Assert.Equal(3, board.Users.Count);
        Assert.Equal(9, board.Items.Count);
        Assert.Equal(9, board.Items.Select(i => i.Id).Distinct().Count());
        Assert.All(board.Items, item =>
        {
            Assert.False(string.IsNullOrWhiteSpace(item.Title));
            Assert.Contains(board.Users, user => user.Id == item.AssigneeId);
        });
    }
}
