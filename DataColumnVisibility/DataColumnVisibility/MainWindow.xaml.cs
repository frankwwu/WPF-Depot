using System.Windows;
using System.ComponentModel.Composition;
using DataColumnVisibility.ViewModels;
using System.Diagnostics.CodeAnalysis;

namespace DataColumnVisibility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();           
        }

        [Import]
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly", Justification = "Needs to be a property to be composed by MEF")]
        ViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }
    }
}
