using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RotatedDataGridDemo
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {          
            Data = new List<DataItem>();
            DataItem item = new DataItem();
            item.DateTime = DateTime.Now;
            item.Name = "American Software Bug Insurance";
            item.Analysis = 5;
            item.Rate = 8;
            item.Review = 185;
            item.Status = "Final";
            item.Version = "2.0";
            item.Comment = "Software bugs can cost companies billions of dollars in repairs, lawsuits, and sales lost.";
            Data.Add(item);
        }

        public List<DataItem> Data { get; set; }


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
