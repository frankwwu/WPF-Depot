using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace DataGridCopy.Behaviors
{
    // Usage:
    //<DataGrid.ContextMenu>
    //    <ContextMenu >
    //        <MenuItem Header="Copy to clipboard" >
    //            <i:Interaction.Behaviors>
    //                <beh:DataGridCopyMenuBehavior />
    //            </i:Interaction.Behaviors>
    //        </MenuItem>
    //    </ContextMenu>
    //</DataGrid.ContextMenu>

    public class DataGridCopyMenuBehavior : Behavior<MenuItem>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Click += OnClick;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Click -= OnClick;
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                DataGrid dg = (mi.Parent as ContextMenu).PlacementTarget as DataGrid;
                if (dg != null)
                {
                    DataGridSelectionMode selectionMode = dg.SelectionMode;
                    dg.SelectionMode = DataGridSelectionMode.Extended;
                    dg.SelectAllCells();
                    dg.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                    ApplicationCommands.Copy.Execute(null, dg);
                    dg.UnselectAllCells();
                    dg.SelectionMode = selectionMode;
                }
            }
        }
    }
}
