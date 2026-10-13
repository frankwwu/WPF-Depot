using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace PopupApp
{
    public class ViewModel : ViewModelBase
    {
        public ViewModel() : this(new MyDataService()) { }

        public ViewModel(MyDataService service)
        {
            Data = new ObservableCollection<MyData>(service.Data);
            BrushList = service.BrushList;
            selectedBrush = BrushList.FirstOrDefault(b => b.Name == nameof(System.Windows.Media.Brushes.Black))
                            ?? BrushList.First();

            foreach (var item in Data)
                item.PropertyChanged += OnItemPropertyChanged;

            SelectAllCommand = new RelayCommand(() => SetAll(true), () => SelectedCount < Data.Count);
            ClearCommand = new RelayCommand(() => SetAll(false), () => SelectedCount > 0);
            CloseCommand = new RelayCommand(() => IsItemsControlOpen = false);
        }

        public ObservableCollection<MyData> Data { get; }

        public IReadOnlyList<NamedBrush> BrushList { get; }

        public ICommand SelectAllCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand CloseCommand { get; }

        private NamedBrush selectedBrush;
        public NamedBrush SelectedBrush
        {
            get => selectedBrush;
            set
            {
                // ListBox pushes null when the selection is cleared; keep the last real colour instead.
                if (value != null)
                    SetProperty(ref selectedBrush, value);
            }
        }

        private string text = "Popup window example...";
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        private bool isItemsControlOpen;
        public bool IsItemsControlOpen
        {
            get => isItemsControlOpen;
            set => SetProperty(ref isItemsControlOpen, value);
        }

        public int SelectedCount => Data.Count(d => d.IsSelected);

        /// <summary>Text shown on the drop-down buttons.</summary>
        public string SelectionSummary
        {
            get
            {
                int count = SelectedCount;
                if (count == 0) return "None selected";
                if (count == Data.Count) return $"All selected ({count})";
                return $"{count} of {Data.Count} selected";
            }
        }

        private void SetAll(bool value)
        {
            foreach (var item in Data)
                item.IsSelected = value;
        }

        private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MyData.IsSelected))
            {
                OnPropertyChanged(nameof(SelectedCount));
                OnPropertyChanged(nameof(SelectionSummary));
            }
        }
    }
}
