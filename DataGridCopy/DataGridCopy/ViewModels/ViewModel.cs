using System.Collections.Generic;
using System.ComponentModel.Composition;
using DataGridCopy.Models;
using DataGridCopy.Services;

namespace DataGridCopy.ViewModels
{
    [Export(typeof(ViewModel))]
    public class ViewModel
    {
        private readonly IDataService _dataService;

        [ImportingConstructor]
        public ViewModel(IDataService dataService)
        {
            _dataService = dataService;
            Persons = _dataService.Persons;
        }

        public List<Person> Persons { get; private set; }
    }
}
