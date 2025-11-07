using DataColumnVisibility.Models;
using DataColumnVisibility.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace DataColumnVisibility.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        private IDataService _dataService;
        private readonly ObservableCollection<DataItem> _model;
        static DataGridContextHelper dc = new DataGridContextHelper();

        public ViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _model = dataService.GetModel();
            DataItemsCV = new ListCollectionView(_model);
        }

        public ICollectionView DataItemsCV { get; private set; }

        private bool _isIdColVisible = true;

        public bool IsIdColVisible
        {
            get => _isIdColVisible;
            set
            {
                _isIdColVisible = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isNameColVisible = true;

        public bool IsNameColVisible
        {
            get => _isNameColVisible;
            set
            {
                _isNameColVisible = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
