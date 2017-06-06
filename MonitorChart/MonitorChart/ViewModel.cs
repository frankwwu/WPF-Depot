using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace MonitorChart
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Service service;    

        public ViewModel()
        {
            service = new Service();
            Data = service.Data;
            DataTitle = "Data";
        }

        public ObservableCollection<Point> Data { get; set; }

        public string DataTitle { get; set; }

        private bool isMonitoring;
        public bool IsMonitoring
        {
            get
            {
                return isMonitoring;
            }
            set
            {
                isMonitoring = value;
                if (isMonitoring)
                {
                    service.Start();
                }
                else
                {
                    service.Stop();
                }
                NotifyPropertyChanged("IsMonitoring");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
