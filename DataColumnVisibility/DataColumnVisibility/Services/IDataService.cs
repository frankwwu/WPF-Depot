using System.Collections.ObjectModel;
using DataColumnVisibility.Models;

namespace DataColumnVisibility.Services
{
    /// <summary>
    /// Data service interface.
    /// </summary>
    public interface IDataService
    {
        ObservableCollection<DataItem> GetModel();
    }
}
