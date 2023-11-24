using System;
using System.Globalization;
using System.Windows.Data;

namespace DynamicDataCellType
{
    public class DayOfWeekEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;

            return Enum.GetName(typeof(DayOfWeek), (DayOfWeek)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
