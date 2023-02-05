using HorizontalGroupPanel.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows.Data;

namespace HorizontalGroupPanel.ViewModels
{
    public class ViewModel
    {
        public ViewModel()
        {
            Statuses = new ObservableCollection<StatusCode>();
            foreach (HttpStatusCode s in Enum.GetValues <HttpStatusCode>())
            {
                StatusCode pair = new StatusCode((int)s, s.ToString());                         
                Statuses.Add(pair);
            }

            CollectionView = CollectionViewSource.GetDefaultView(Statuses);
            CollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
        }

        public ObservableCollection<StatusCode> Statuses { get; set; }    

        public ICollectionView CollectionView { get; set; }


    }
}
