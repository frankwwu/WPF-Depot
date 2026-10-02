using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Kanban.Models;

namespace Kanban.Converters;

/// <summary>Maps a <see cref="WorkItemStatus"/> to its accent color (column dot, card edge).</summary>
[ValueConversion(typeof(WorkItemStatus), typeof(Brush))]
public sealed class StatusBrushConverter : IValueConverter
{
    private static readonly Brush New = FrozenBrush.From("#6366F1");        // indigo
    private static readonly Brush InProgress = FrozenBrush.From("#F59E0B"); // amber
    private static readonly Brush Done = FrozenBrush.From("#10B981");       // emerald
    private static readonly Brush Unknown = FrozenBrush.From("#9CA3AF");

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value switch
    {
        WorkItemStatus.New => New,
        WorkItemStatus.InProgress => InProgress,
        WorkItemStatus.Done => Done,
        _ => Unknown,
    };

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
