using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace WpfCustomWindow
{
    public class WindowExt : Window
    {
        //public WindowExt()
        //    : base()
        //{                      
        //}

        static WindowExt()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowExt),
                new FrameworkPropertyMetadata(typeof(WindowExt)));
        }
      
        public event EventHandler<GenericEventArgs<bool>> CloseButtonClicked;

        public bool ShowCloseButton
        {
            get { return (bool)GetValue(ShowCloseButtonProperty); }
            set { SetValue(ShowCloseButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowCloseButtonProperty =
            DependencyProperty.Register("ShowCloseButton", typeof(bool), typeof(WindowExt), new UIPropertyMetadata(ShowCloseButtonChanged));

        private static void ShowCloseButtonChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            WindowExt window = target as WindowExt;
            if (window != null)
            {
                Button btnClose = window.GetTemplateChild("btnClose") as Button;
                if (btnClose != null)
                {
                    btnClose.Visibility = window.ShowCloseButton ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        public override void OnApplyTemplate()
        {
            Button btnMinimize = GetTemplateChild("btnMinimize") as Button;
            if (btnMinimize != null)
                btnMinimize.Click += OnMinimizeClick;

            Button btnRestore = GetTemplateChild("btnRestore") as Button;
            if (btnRestore != null)
                btnRestore.Click += OnRestoreClick;

            Button btnClose = GetTemplateChild("btnClose") as Button;
            if (btnClose != null)
                btnClose.Click += OnCloseClick;

            Rectangle rect = GetTemplateChild("rectMove") as Rectangle;
            if (rect != null)
                rect.MouseLeftButtonDown += OnMouseLeftButtonDown;

            base.OnApplyTemplate();
        }

        #region Event Handlers

        protected void OnMinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        protected void OnRestoreClick(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        protected void OnCloseClick(object sender, RoutedEventArgs e)
        {
            if (CloseButtonClicked != null)
            {
                GenericEventArgs<bool> args = new GenericEventArgs<bool>(true);
                CloseButtonClicked(this, args);
                if (args.EventData)
                {
                    Close();
                }
            }
            else
            {
                Close();
            }
        }

        protected void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        #endregion
    }
}
