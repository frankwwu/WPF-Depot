using System.Windows;

namespace PopupApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class View : Window
    {
        public View()
        {
            InitializeComponent();
            this.DataContext = new ViewModel();
        }
    }
}
