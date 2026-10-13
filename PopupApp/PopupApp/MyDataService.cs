using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace PopupApp
{
    public sealed record NamedBrush(string Name, Brush Brush);

    public class MyDataService
    {
        public List<MyData> Data { get; } = new()
        {
            new MyData { Name = "Icebreaker", IsSelected = true },
            new MyData { Name = "Organize Your Speech", IsSelected = true },
            new MyData { Name = "Get to the Point", IsSelected = true },
            new MyData { Name = "How to Say It", IsSelected = true },
            new MyData { Name = "Your Body Speaks" },
            new MyData { Name = "Vocal Variety" },
            new MyData { Name = "Research Your Topic", IsSelected = true },
            new MyData { Name = "Get Comfortable with Visual Aids" },
            new MyData { Name = "Persuade with Power" },
            new MyData { Name = "Inspire Your Audience" },
        };

        private static readonly List<NamedBrush> brushes = LoadBrushes();

        public IReadOnlyList<NamedBrush> BrushList => brushes;

        private static List<NamedBrush> LoadBrushes()
        {
            return typeof(Brushes)
                .GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(p => p.PropertyType == typeof(SolidColorBrush) && p.Name != nameof(Brushes.Transparent))
                .Select(p => new NamedBrush(p.Name, (Brush)p.GetValue(null)!))
                .ToList();
        }
    }
}
