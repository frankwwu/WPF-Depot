using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Data;
using DataColumnVisibility.Models;
using DataColumnVisibility.Services;

namespace DataColumnVisibility.ViewModels
{
    [Export(typeof(ViewModel))]
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly IDataService _dataService;
        private readonly ObservableCollection<DataItem> model;
        private bool isNameVisible = true;
        private bool isDescriptionVisible = true;
        private bool isSelectedVisible = true;
        static DataGridContextHelper dc = new DataGridContextHelper();

        [ImportingConstructor]
        public ViewModel(IDataService dataService)
        {
            _dataService = dataService;
            this.model = _dataService.GetModel();
            this.DataItemsCV = new ListCollectionView(model);
        }

        public ICollectionView DataItemsCV { get; set; }

        public bool IsNameVisible
        {
            get
            {
                return isNameVisible;
            }
            set
            {
                isNameVisible = value;
                NotifyPropertyChanged("IsNameVisible");
            }
        }

        public bool IsDescriptionVisible
        {
            get
            {
                return isDescriptionVisible;
            }
            set
            {
                isDescriptionVisible = value;
                NotifyPropertyChanged("IsDescriptionVisible");
            }
        }

        public bool IsSelectedVisible
        {
            get
            {
                return isSelectedVisible;
            }
            set
            {
                isSelectedVisible = value;
                NotifyPropertyChanged("IsSelectedVisible");
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
