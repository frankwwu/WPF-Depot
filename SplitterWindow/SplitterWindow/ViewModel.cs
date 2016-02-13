using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SplitterWindow
{
    public class ViewModel : INotifyPropertyChanged
    {
        private GridLength collapsedPanelWidth = new GridLength(0);

        private bool isRightPanelExpended = true;

        public bool IsRightPanelExpended
        {
            get
            {
                return isRightPanelExpended;
            }
            set
            {
                isRightPanelExpended = value;
                OnPropertyChanged();
                OnPropertyChanged("RightPanelWidth");
            }
        }

        private GridLength rightPanelWidth = new GridLength(1, GridUnitType.Star);

        public GridLength RightPanelWidth
        {
            get
            {
                if (IsRightPanelExpended)
                {
                    return rightPanelWidth;
                }
                else
                {
                    return collapsedPanelWidth;
                }
            }
            set
            {
                if (IsRightPanelExpended)
                {
                    rightPanelWidth = value;
                }
                OnPropertyChanged();
            }
        }

        private bool isBottomPanelExpended = true;

        public bool IsBottomPanelExpended
        {
            get
            {
                return isBottomPanelExpended;
            }
            set
            {
                isBottomPanelExpended = value;
                OnPropertyChanged();
                OnPropertyChanged("BottomPanelWidth");
            }
        }

        private GridLength bottomPanelWidth = new GridLength(1, GridUnitType.Star);

        public GridLength BottomPanelWidth
        {
            get
            {
                if (IsBottomPanelExpended)
                {
                    return bottomPanelWidth;
                }
                else
                {
                    return collapsedPanelWidth;
                }
            }
            set
            {
                if (IsBottomPanelExpended)
                {
                    bottomPanelWidth = value;
                }
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] String name = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
