using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace DataGridColumnBinding
{
    /// <summary>
    /// Usage: <DataGrid dp:DataGridDynamicColumns.BindableColumns="{Binding ColumnCollection}"
    ///        where ColumnCollection is Collection<DataGridColumn>
    /// </summary>
    public class DataGridDynamicColumns
    {
        public static readonly DependencyProperty BindableColumnsProperty = DependencyProperty.RegisterAttached("BindableColumns",
                                               typeof(Collection<DataGridColumn>),
                                               typeof(DataGridDynamicColumns),
                                               new UIPropertyMetadata(null, BindableColumnsPropertyChanged));
        private static void BindableColumnsPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = source as DataGrid;
            Collection<DataGridColumn> columns = e.NewValue as Collection<DataGridColumn>;
            if (columns == null)
            {
                return;
            }
            dataGrid.Columns.Clear();
            foreach (DataGridColumn column in columns)
            {
                dataGrid.Columns.Add(column);
            }
        }

        public static void SetBindableColumns(DependencyObject element, Collection<DataGridColumn> value)
        {
            element.SetValue(BindableColumnsProperty, value);
        }

        public static Collection<DataGridColumn> GetBindableColumns(DependencyObject element)
        {
            return element.GetValue(BindableColumnsProperty) as Collection<DataGridColumn>;
        }
    }
}

