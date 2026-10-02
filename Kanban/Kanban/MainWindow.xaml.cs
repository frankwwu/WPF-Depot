using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Kanban.ViewModels;

namespace Kanban;

/// <summary>
/// View-only code: drag-and-drop plumbing and opening a menu. Everything that changes the board
/// goes through <see cref="BoardViewModel"/>.
/// </summary>
public partial class MainWindow : Window
{
    private const string DragFormat = "Kanban.CardViewModel";

    private Point _dragOrigin;
    private CardViewModel? _dragCandidate;

    public MainWindow() => InitializeComponent();

    // ---- the "..." button ----------------------------------------------------------------

    private void CardMenuButton_Click(object sender, RoutedEventArgs e)
    {
        // The button's Tag is the card's root element, which owns the (right-click) context menu.
        if (sender is not Button { Tag: FrameworkElement { ContextMenu: { } menu } } button)
        {
            return;
        }

        menu.PlacementTarget = button;
        menu.Placement = PlacementMode.Bottom;
        menu.IsOpen = true;
    }

    // ---- drag and drop -------------------------------------------------------------------

    private void CardList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _dragOrigin = e.GetPosition(null);

        // Remember which card the press landed on, but don't start dragging until the mouse moves
        // a bit (so clicks and double-clicks still work). Presses on the "..." button never drag.
        var source = e.OriginalSource as DependencyObject;
        _dragCandidate = source is not null && FindAncestor<ButtonBase>(source) is null
            ? FindAncestor<ListBoxItem>(source)?.DataContext as CardViewModel
            : null;
    }

    private void CardList_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed || _dragCandidate is not { } card)
        {
            return;
        }

        var delta = e.GetPosition(null) - _dragOrigin;
        if (Math.Abs(delta.X) < SystemParameters.MinimumHorizontalDragDistance &&
            Math.Abs(delta.Y) < SystemParameters.MinimumVerticalDragDistance)
        {
            return;
        }

        _dragCandidate = null;
        DragDrop.DoDragDrop((DependencyObject)sender, new DataObject(DragFormat, card), DragDropEffects.Move);
    }

    private void CardList_DragOver(object sender, DragEventArgs e)
    {
        var accepts = e.Data.GetDataPresent(DragFormat);
        e.Effects = accepts ? DragDropEffects.Move : DragDropEffects.None;

        if (accepts && sender is ListBox listBox)
        {
            listBox.Background = (Brush)FindResource("DropTargetBrush");
        }

        e.Handled = true;
    }

    private void CardList_DragLeave(object sender, DragEventArgs e)
    {
        // DragLeave also bubbles up from child cards; only un-highlight if we really left the list.
        if (sender is ListBox listBox && !new Rect(listBox.RenderSize).Contains(e.GetPosition(listBox)))
        {
            listBox.ClearValue(Control.BackgroundProperty);
        }
    }

    private void CardList_Drop(object sender, DragEventArgs e)
    {
        if (sender is not ListBox { DataContext: ColumnViewModel column } listBox)
        {
            return;
        }

        listBox.ClearValue(Control.BackgroundProperty);

        if (e.Data.GetData(DragFormat) is CardViewModel card && DataContext is BoardViewModel board)
        {
            board.MoveCard(card, column.Status, GetInsertionSlot(listBox, e));
            e.Handled = true;
        }
    }

    /// <summary>
    /// The slot to drop into: the index of the first card whose vertical midpoint is below the
    /// pointer, or the item count if the pointer is below every card.
    /// </summary>
    private static int GetInsertionSlot(ListBox listBox, DragEventArgs e)
    {
        for (var i = 0; i < listBox.Items.Count; i++)
        {
            if (listBox.ItemContainerGenerator.ContainerFromIndex(i) is FrameworkElement container &&
                e.GetPosition(container).Y < container.ActualHeight / 2)
            {
                return i;
            }
        }

        return listBox.Items.Count;
    }

    private static T? FindAncestor<T>(DependencyObject? start) where T : DependencyObject
    {
        for (var current = start; current is not null;)
        {
            if (current is T match)
            {
                return match;
            }

            // Run/Span and friends are ContentElements: they have no visual parent, only a logical one.
            current = current is Visual or System.Windows.Media.Media3D.Visual3D
                ? VisualTreeHelper.GetParent(current)
                : LogicalTreeHelper.GetParent(current);
        }

        return null;
    }
}
