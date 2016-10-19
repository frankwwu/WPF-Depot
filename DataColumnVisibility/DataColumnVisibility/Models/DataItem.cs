using System.ComponentModel;

namespace DataColumnVisibility.Models
{
    public class DataItem : INotifyPropertyChanged
    {
        private bool isEditable;

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsEditable
        {
            get
            {
                return isEditable;
            }
            set
            {
                isEditable = value;
                NotifyPropertyChanged("IsEditable");
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
