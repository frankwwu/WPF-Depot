namespace Kanban.Models;

/// <summary>The board's columns, in display order.</summary>
public enum WorkItemStatus
{
    New,
    InProgress,
    Done,
}

public static class WorkItemStatusExtensions
{
    public static string DisplayName(this WorkItemStatus status) => status switch
    {
        WorkItemStatus.New => "New",
        WorkItemStatus.InProgress => "In Progress",
        WorkItemStatus.Done => "Done",
        _ => status.ToString(),
    };
}
