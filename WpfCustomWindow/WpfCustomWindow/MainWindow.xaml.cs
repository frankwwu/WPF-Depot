using System.Windows;

namespace WpfCustomWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowExt
    {
        public MainWindow()
        {
            InitializeComponent();
            this.CloseButtonClicked += MainWindow_CloseButtonClicked;
        }

        void MainWindow_CloseButtonClicked(object sender, GenericEventArgs<bool> e)
        {
              MessageBoxResult res = MessageBox.Show("Are you sure you want to close the window?", "Warning", MessageBoxButton.YesNo);
              if (res != MessageBoxResult.Yes)
              {
                  e.EventData = false;
              }
        }
    }
}
