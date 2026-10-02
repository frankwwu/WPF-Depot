using System.Windows.Media;

namespace Kanban.Converters;

internal static class FrozenBrush
{
    /// <summary>Creates a frozen (immutable, thread-safe, cheap) brush from a "#RRGGBB" string.</summary>
    public static SolidColorBrush From(string hex)
    {
        var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
        brush.Freeze();
        return brush;
    }
}
