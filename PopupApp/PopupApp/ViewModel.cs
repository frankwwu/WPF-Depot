using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Media;
using System;

namespace PopupApp
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            var service = new MyDataService();
            Data = service.Data;
            BrushList = service.BrushList;
        }

        public List<MyData> Data { get; set; }

        public List<Tuple<string, Brush>> BrushList { get; private set; }

        private Tuple<string, Brush> selectedBrush;

        public Tuple<string, Brush> SelectedBrush
        {
            get
            {
                return selectedBrush;
            }
            set
            {
                selectedBrush = value;
                NotifyPropertyChanged("SelectedBrush");
            }
        }

        private string text = "Popup window example...";
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                NotifyPropertyChanged("Text");
            }
        }

        private bool isItemsControlOpen;
        public bool IsItemsControlOpen
        {
            get
            {
                return isItemsControlOpen;
            }
            set
            {
                isItemsControlOpen = value;
                NotifyPropertyChanged("IsItemsControlOpen");
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
