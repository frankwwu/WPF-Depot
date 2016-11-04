using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using DataColumnVisibility.Models;
using DataColumnVisibility.Services;

namespace DataColumnVisibility.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        private IDataService dataService;
        private readonly ObservableCollection<DataItem> _model;
        static DataGridContextHelper dc = new DataGridContextHelper();

        public ViewModel()
        {
            dataService = new DataService();
            _model = dataService.GetModel();
            DataItemsCV = new ListCollectionView(_model);
        }

        public ICollectionView DataItemsCV { get; private set; }

        private bool isIdColVisible = true;

        public bool IsIdColVisible
        {
            get
            {
                return isIdColVisible;
            }
            set
            {
                isIdColVisible = value;
                NotifyPropertyChanged("IsIdColVisible");
            }
        }

        private bool isNameColVisible = true;

        public bool IsNameColVisible
        {
            get
            {
                return isNameColVisible;
            }
            set
            {
                isNameColVisible = value;
                NotifyPropertyChanged("IsNameColVisible");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
