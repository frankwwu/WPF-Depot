using System.Windows;
using Kanban.Views;

namespace Kanban.Services;

/// <summary>WPF implementation of <see cref="IDialogService"/>.</summary>
public sealed class WpfDialogService : IDialogService
{
    private static Window? Parent => Application.Current?.MainWindow;

    public string? PromptForText(string title, string prompt, string initialText)
    {
        var dialog = new TextPromptWindow(title, prompt, initialText) { Owner = Parent };
        return dialog.ShowDialog() == true ? dialog.Text : null;
    }

    public bool Confirm(string title, string message) =>
        Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes;

    public void ShowError(string message) =>
        Show(message, "Kanban", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

    private static MessageBoxResult Show(
        string message, string title, MessageBoxButton buttons, MessageBoxImage image, MessageBoxResult defaultResult) =>
        Parent is { } owner
            ? MessageBox.Show(owner, message, title, buttons, image, defaultResult)
            : MessageBox.Show(message, title, buttons, image, defaultResult);
}
