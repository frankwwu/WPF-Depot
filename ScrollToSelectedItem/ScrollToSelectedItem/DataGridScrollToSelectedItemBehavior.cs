using Microsoft.Xaml.Behaviors;
using System;
using System.Windows.Controls;

namespace ScrollToSelectedItem
{
    public class DataGridScrollToSelectedItemBehavior : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }

        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid)
            {
                DataGrid? dataGrid = sender as DataGrid;
                if (dataGrid?.SelectedItem != null)
                {
                    dataGrid.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        dataGrid.UpdateLayout();
                        if (dataGrid.SelectedItem != null)
                        {
                            dataGrid.ScrollIntoView(dataGrid.SelectedItem);
                        }
                    }));
                }
            }
        }
    }
}

