using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace GridLayoutTransform
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }       

        public bool IsLayoutRotated
        {
            get { return Properties.Settings.Default.IsLayoutRotated; }
            set { Properties.Settings.Default.IsLayoutRotated = value; Properties.Settings.Default.Save();  NotifyPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
