using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using DataColumnVisibility.Models;

namespace DataColumnVisibility.Services
{
    /// <summary>
    /// Dummy data service class. Provides a dummy data model.
    /// </summary>
    [Export(typeof(IDataService))]
    public class DataService : IDataService
    {
        private ObservableCollection<DataItem> model;

        public ObservableCollection<DataItem> GetModel()
        {
            if (model == null)
            {
                model = new ObservableCollection<DataItem>();

                for (int i = 0; i < 20; i++)
                {
                    model.Add(new DataItem() { Id = i, Name = string.Format( "Name {0}  ", i), Description = string.Format("Description {0}  ", i)});
                }              
            }

            return model;
        }
    }
}
