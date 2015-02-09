using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DataTemplateSelectorDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            Data = new ObservableCollection<DataItem>();
            Data.Add(new DataItem() { ID = 1, Value = 12, Description = "Integer" });
            Data.Add(new DataItem() { ID = 2, Value = "A string walks into a bar...", Description = "String" });
            Data.Add(new DataItem() { ID = 3, Value = DateTime.Now, Description = "Display a DatePicker for the DateTime type" });
            Data.Add(new DataItem() { ID = 4, Value = DayOfWeek.Saturday, Description = "Display a  ComboBox for the DayOfWeek enum" });
            Data.Add(new DataItem() { ID = 5, Value = (decimal)260.95, Description = "Money" });
            Data.Add(new DataItem() { ID = 6, Value = Enum.GetNames(typeof(DayOfWeek)).ToList(), Description = "Display a ListBox for the IList" });
        }

        public ObservableCollection<DataItem> Data { get; set; }
    }
}
