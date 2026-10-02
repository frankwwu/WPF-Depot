using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Kanban.Models;

namespace Kanban.Converters;

/// <summary>Gives each <see cref="User"/> a stable avatar color (white text stays readable on all of them).</summary>
[ValueConversion(typeof(User), typeof(Brush))]
public sealed class UserColorConverter : IValueConverter
{
    private static readonly Brush[] Palette =
    [
        FrozenBrush.From("#7C3AED"), // violet
        FrozenBrush.From("#DB2777"), // pink
        FrozenBrush.From("#0E7490"), // cyan
        FrozenBrush.From("#047857"), // green
        FrozenBrush.From("#B45309"), // amber
        FrozenBrush.From("#DC2626"), // red
        FrozenBrush.From("#2563EB"), // blue
        FrozenBrush.From("#A21CAF"), // fuchsia
    ];

    private static readonly Brush Fallback = FrozenBrush.From("#6B7280");

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is User user ? Palette[(uint)user.Id % (uint)Palette.Length] : Fallback;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
