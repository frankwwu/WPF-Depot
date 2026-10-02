using System.ComponentModel;
using Kanban.Models;
using Kanban.Mvvm;

namespace Kanban.ViewModels;

/// <summary>
/// A card on the board: a <see cref="WorkItem"/> plus the commands that act on it.
/// Having the commands here (rather than on the board, taking the item as a parameter) lets a
/// card's context menu bind straight to them.
/// </summary>
public sealed class CardViewModel : ObservableObject
{
    private readonly BoardViewModel _board;

    public CardViewModel(WorkItem item, BoardViewModel board)
    {
        Item = item;
        _board = board;

        EditCommand = new DelegateCommand(() => board.EditTitle(this));
        DeleteCommand = new DelegateCommand(() => board.Delete(this));
        MoveToCommand = new DelegateCommand<WorkItemStatus>(
            status => board.MoveCard(this, status),
            status => status != Item.Status);
        AssignCommand = new DelegateCommand<User>(
            user => board.Assign(this, user),
            user => user != Item.Assignee);
        UnassignCommand = new DelegateCommand(
            () => board.Assign(this, null),
            () => Item.Assignee is not null);

        item.PropertyChanged += OnItemPropertyChanged;
    }

    public WorkItem Item { get; }

    public string AssigneeName => Item.Assignee?.FullName ?? "Unassigned";

    /// <summary>The columns a card can be moved to (for the "Move to" menu).</summary>
    public IReadOnlyList<ColumnViewModel> MoveTargets => _board.Columns;

    /// <summary>The people a card can be assigned to (for the "Assign to" menu).</summary>
    public IReadOnlyList<User> AssignTargets => _board.Users;

    public DelegateCommand EditCommand { get; }

    public DelegateCommand DeleteCommand { get; }

    /// <summary>Parameter: the <see cref="WorkItemStatus"/> to move to. Disabled for the current status.</summary>
    public DelegateCommand<WorkItemStatus> MoveToCommand { get; }

    /// <summary>Parameter: the <see cref="User"/> to assign. Disabled for the current assignee.</summary>
    public DelegateCommand<User> AssignCommand { get; }

    public DelegateCommand UnassignCommand { get; }

    private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(WorkItem.Status):
                MoveToCommand.RaiseCanExecuteChanged();
                break;

            case nameof(WorkItem.Assignee):
                OnPropertyChanged(nameof(AssigneeName));
                AssignCommand.RaiseCanExecuteChanged();
                UnassignCommand.RaiseCanExecuteChanged();
                break;
        }
    }
}
