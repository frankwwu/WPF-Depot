using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace DataTemplateSelectorDemo
{
    public class CellDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StringTemplate { get; set; }

        public DataTemplate DateTimeTemplate { get; set; }

        public DataTemplate DayOfWeekTemplate { get; set; }

        public DataTemplate MoneyTemplate { get; set; }

        public DataTemplate ListTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if ((item != null) && (item is DataItem))
            {
                DataItem dataItem = item as DataItem;
                if (dataItem.Value is int)
                {
                    return StringTemplate;
                }
                else if (dataItem.Value is string)
                {
                    return StringTemplate;
                }
                else if (dataItem.Value is DateTime)
                {
                    return DateTimeTemplate;
                }
                else if (dataItem.Value is DayOfWeek)
                {
                    return DayOfWeekTemplate;
                }
                else if (dataItem.Value is decimal)
                {
                    return MoneyTemplate;
                }
                else if (dataItem.Value is IList)
                {
                    return ListTemplate;
                }
                else
                {
                    return base.SelectTemplate(item, container);
                }
            }
            else
            {
                return base.SelectTemplate(item, container);
            }
        }
    }
}
