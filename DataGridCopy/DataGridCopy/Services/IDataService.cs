using System.Collections.Generic;
using DataGridCopy.Models;

namespace DataGridCopy.Services
{
    public interface IDataService
    {
        List<Person> Persons { get; }
    }
}

