namespace Kanban.Services;

/// <summary>
/// The few bits of UI a view model needs. Keeping this behind an interface means
/// the view models stay free of <c>MessageBox</c>/<c>Window</c> and can be unit tested.
/// </summary>
public interface IDialogService
{
    /// <summary>Asks for a line of text. Returns <see langword="null"/> if the user cancels.</summary>
    string? PromptForText(string title, string prompt, string initialText);

    bool Confirm(string title, string message);

    void ShowError(string message);
}
