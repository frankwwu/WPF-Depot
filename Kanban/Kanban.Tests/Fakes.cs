using Kanban.Models;
using Kanban.Services;

namespace Kanban.Tests;

internal sealed class FakeRepository : IBoardRepository
{
    public BoardSnapshot? Saved { get; private set; }

    public int SaveCount { get; private set; }

    /// <summary>When set, <see cref="Save"/> throws this instead of recording the snapshot.</summary>
    public Exception? FailWith { get; set; }

    public BoardSnapshot? Load() => Saved;

    public void Save(BoardSnapshot snapshot)
    {
        if (FailWith is not null)
        {
            throw FailWith;
        }

        Saved = snapshot;
        SaveCount++;
    }
}

internal sealed class FakeDialogs : IDialogService
{
    /// <summary>What <see cref="PromptForText"/> returns. <see langword="null"/> simulates Cancel.</summary>
    public string? PromptResult { get; set; }

    public bool ConfirmResult { get; set; } = true;

    public List<string> Errors { get; } = [];

    public string? PromptForText(string title, string prompt, string initialText) => PromptResult;

    public bool Confirm(string title, string message) => ConfirmResult;

    public void ShowError(string message) => Errors.Add(message);
}
