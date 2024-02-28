using Microsoft.Xaml.Behaviors;
using System;
using System.Windows.Controls;

namespace ScrollToSelectedItem
{
    public class ListBoxScrollToSelectedItemBehavior : Behavior<ListBox>
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
            if (sender is ListBox)
            {
                ListBox? listBox = sender as ListBox;
                if (listBox?.SelectedItem != null)
                {
                    listBox.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        listBox.UpdateLayout();
                        if (listBox.SelectedItem != null)
                        {
                            listBox.ScrollIntoView(listBox.SelectedItem);
                        }
                    }));
                }
            }
        }
    }
}

