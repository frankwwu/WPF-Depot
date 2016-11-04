using System.Collections.ObjectModel;
using DataColumnVisibility.Models;

namespace DataColumnVisibility.Services
{
    public class DataService : IDataService
    {
        private ObservableCollection<DataItem> model;

        public ObservableCollection<DataItem> GetModel()
        {
            if (model == null)
            {
                model = new ObservableCollection<DataItem>();
                for (int i = 1; i < 10; i++)
                {
                    model.Add(new DataItem() { Id = i, Name = string.Format("Item {0}", i), Description = string.Format("Description {0}", i) });
                }
            }

            return model;
        }
    }
}
