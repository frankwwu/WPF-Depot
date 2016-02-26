using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace AutoLayoutGrid
{
    public class AutoLayoutGridBehavior : Behavior<FrameworkElement>
    {
        private bool isLandscape = true;

        protected override void OnAttached()
        {
            AssociatedObject.SizeChanged += OnSizeChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SizeChanged -= OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            FrameworkElement ele = sender as FrameworkElement;
            bool landscape = ele.ActualWidth > ele.ActualHeight;
            if (landscape == isLandscape)
                return;

            Grid grid = GetVisualChild<Grid>(ele);
            if (grid == null)
                return;

            isLandscape = landscape;
            if (grid.Children.Count == 3)
            {
                FrameworkElement ele1 = grid.Children[0] as FrameworkElement;
                GridSplitter splitter = grid.Children[1] as GridSplitter;
                FrameworkElement ele2 = grid.Children[2] as FrameworkElement;

                if ((ele1 != null) && (splitter != null) && (ele2 != null))
                {
                    if (!isLandscape)
                    {
                        // Grid Transform
                        TransformGroup group = new TransformGroup();
                        RotateTransform rotate = new RotateTransform(90);
                        MatrixTransform mat = new MatrixTransform(-1, 0, 0, 1, 0, 0);
                        group.Children.Add(rotate);
                        group.Children.Add(mat);
                        grid.LayoutTransform = group;

                        // First Panel Transform
                        TransformGroup group1 = new TransformGroup();
                        RotateTransform rotate1 = new RotateTransform(-90);
                        ScaleTransform scale1 = new ScaleTransform(1, -1);
                        group1.Children.Add(rotate1);
                        group1.Children.Add(scale1);
                        ele1.LayoutTransform = group1;

                        // Set cursor for the GridSplitter
                        splitter.Cursor = Cursors.SizeNS;

                        // Second Panel Transform
                        TransformGroup group2 = new TransformGroup();
                        RotateTransform rotate2 = new RotateTransform(-90);
                        ScaleTransform scale2 = new ScaleTransform(1, -1);
                        group2.Children.Add(rotate2);
                        group2.Children.Add(scale2);
                        ele2.LayoutTransform = group2;
                    }
                    else
                    {
                        // Grid Transform
                        TransformGroup group = new TransformGroup();
                        RotateTransform rt = new RotateTransform(180);
                        MatrixTransform mat = new MatrixTransform(-1, 0, 0, 1, 0, 0);
                        group.Children.Add(rt);
                        group.Children.Add(mat);
                        grid.LayoutTransform = group;

                        // First Panel Transform
                        TransformGroup group1 = new TransformGroup();
                        RotateTransform rotate1 = new RotateTransform(0);
                        ScaleTransform scale1 = new ScaleTransform(1, -1);
                        group1.Children.Add(rotate1);
                        group1.Children.Add(scale1);
                        ele1.LayoutTransform = group1;

                        // Set cursor for the GridSplitter
                        splitter.Cursor = Cursors.SizeWE;

                        // Second Panel Transform
                        TransformGroup group2 = new TransformGroup();
                        RotateTransform rotate2 = new RotateTransform(0);
                        ScaleTransform scale2 = new ScaleTransform(1, -1);
                        group2.Children.Add(rotate2);
                        group2.Children.Add(scale2);
                        ele2.LayoutTransform = group2;
                    }
                }
            }
        }

        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                Visual visual = VisualTreeHelper.GetChild(parent, i) as Visual;
                child = visual as T;
                if (child == null)
                    child = GetVisualChild<T>(visual);
                if (child != null)
                    break;
            }
            return child;
        }
    }
}
