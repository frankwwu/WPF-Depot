using Kanban.Mvvm;

namespace Kanban.Models;

public sealed class WorkItem(int id, string title, WorkItemStatus status = WorkItemStatus.New, User? assignee = null)
    : ObservableObject
{
    public int Id { get; } = id;

    public string Title { get; set => SetProperty(ref field, value); } = title;

    public WorkItemStatus Status { get; set => SetProperty(ref field, value); } = status;

    public User? Assignee { get; set => SetProperty(ref field, value); } = assignee;
}
