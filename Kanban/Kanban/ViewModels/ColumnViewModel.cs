using System.Collections.ObjectModel;
using Kanban.Models;
using Kanban.Mvvm;

namespace Kanban.ViewModels;

/// <summary>One column of the board (New / In Progress / Done) and the cards in it, top to bottom.</summary>
public sealed class ColumnViewModel : ObservableObject
{
    public ColumnViewModel(WorkItemStatus status)
    {
        Status = status;
        Items.CollectionChanged += (_, _) => OnPropertyChanged(nameof(Count));
    }

    public WorkItemStatus Status { get; }

    public string Title => Status.DisplayName();

    public ObservableCollection<CardViewModel> Items { get; } = [];

    public int Count => Items.Count;
}
