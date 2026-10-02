using System.Windows;
using Kanban.Services;
using Kanban.ViewModels;

namespace Kanban;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ApplyTheme();

        // Composition root. Three objects don't need a DI container.
        var repository = new JsonBoardRepository(JsonBoardRepository.DefaultPath);
        var board = new BoardViewModel(
            repository.Load() ?? SampleData.Create(),
            repository,
            new WpfDialogService());

        MainWindow = new MainWindow { DataContext = board };
        MainWindow.Show();
    }

    /// <summary>Follows the Windows "app mode" setting as of launch.</summary>
    private void ApplyTheme()
    {
        var dark = SystemTheme.AppsUseDarkMode();

        // The Fluent theme styles the built-in controls (buttons, text boxes, menus, scrollbars)...
        ThemeMode = dark ? ThemeMode.Dark : ThemeMode.Light;

        // ...and these dictionaries provide the board's own colors.
        Resources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri($"pack://application:,,,/Themes/{(dark ? "Dark" : "Light")}.xaml", UriKind.Absolute),
        });
    }
}
