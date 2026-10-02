namespace Kanban.Models;

/// <summary>
/// A plain-data picture of the whole board. This is what gets persisted, and what the
/// <see cref="ViewModels.BoardViewModel"/> is built from. Items are listed column by column,
/// top to bottom, so ordering survives a save/load round trip.
/// </summary>
public sealed record BoardSnapshot
{
    public IReadOnlyList<User> Users { get; init; } = [];

    public IReadOnlyList<WorkItemSnapshot> Items { get; init; } = [];
}

public sealed record WorkItemSnapshot(int Id, string Title, WorkItemStatus Status, int? AssigneeId);
