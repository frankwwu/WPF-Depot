using Microsoft.Win32;

namespace Kanban;

internal static class SystemTheme
{
    /// <summary>True if Windows is set to dark mode for apps.</summary>
    public static bool AppsUseDarkMode()
    {
        using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
        return key?.GetValue("AppsUseLightTheme") is 0;
    }
}
