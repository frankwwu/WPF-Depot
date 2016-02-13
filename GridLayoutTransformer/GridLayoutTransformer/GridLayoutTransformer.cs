using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace GridLayoutTransform
{
    public class GridLayoutTransformer
    {
        public static bool GetIsRotated(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsRotatedProperty);
        }

        public static void SetIsRotated(DependencyObject obj, bool value)
        {
            obj.SetValue(IsRotatedProperty, value);
        }

        public static readonly DependencyProperty IsRotatedProperty =
            DependencyProperty.RegisterAttached("IsRotated", typeof(bool), typeof(GridLayoutTransformer), new UIPropertyMetadata(false, OnIsRotatedChanged));

        static void OnIsRotatedChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {           
            ToggleButton tgl = depObj as ToggleButton;
            Grid grid = FindParent<Grid>(tgl);

            if (grid == null)
                return;

            if (grid.Children.Count == 3)
            {
                FrameworkElement ele1 = grid.Children[0] as FrameworkElement;
                GridSplitter splitter = grid.Children[1] as GridSplitter;
                FrameworkElement ele2 = grid.Children[2] as FrameworkElement;

                if ((ele1 != null) && (splitter != null) && (ele2 != null))
                {
                    if ((bool)e.NewValue)
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

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {         
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null) return null;
         
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }
    }
}
