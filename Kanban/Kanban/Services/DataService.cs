using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using Kanban.Models;

namespace Kanban.Services
{
    [Export(typeof(IDataService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class DataService : IDataService
    {
        public DataService()
        {
        }

        private List<WorkItem> _workItems;

        public List<WorkItem> WorkItems 
        {
            get
            {
                if(_workItems == null)
                {
                    _workItems = new List<WorkItem>();
                    _workItems.Add(new WorkItem() { Id = 20, Status = Statuses[0], Person = Persons[0], Title = "Navigation across swimlanes" });
                    _workItems.Add(new WorkItem() { Id = 22, Status = Statuses[1], Person = Persons[1], Title = "Bug insurance" });
                    _workItems.Add(new WorkItem() { Id = 24, Status = Statuses[2], Person = Persons[2], Title = "Multiple configuration files" });
                    _workItems.Add(new WorkItem() { Id = 25, Status = Statuses[0], Person = Persons[3], Title = "Conditional formatting" });
                    _workItems.Add(new WorkItem() { Id = 30, Status = Statuses[1], Person = Persons[4], Title = "Leap year rules" });
                    _workItems.Add(new WorkItem() { Id = 32, Status = Statuses[2], Person = Persons[0], Title = "Modbus driver error message handling" });
                    _workItems.Add(new WorkItem() { Id = 36, Status = Statuses[0], Person = Persons[1], Title = "Color coding in the editor" });
                    _workItems.Add(new WorkItem() { Id = 37, Status = Statuses[1], Person = Persons[3], Title = "Customizable activation function" });
                    _workItems.Add(new WorkItem() { Id = 38, Status = Statuses[2], Person = Persons[4], Title = "Latex display in the output" });
                    _workItems.Add(new WorkItem() { Id = 80, Status = Statuses[0], Person = Persons[0], Title = "Expression tree filter for search" });
                    _workItems.Add(new WorkItem() { Id = 81, Status = Statuses[1], Person = Persons[1], Title = "Starts interactive session" });
                    _workItems.Add(new WorkItem() { Id = 82, Status = Statuses[2], Person = Persons[2], Title = "Add answers to overleaf" });
                }
                return _workItems;
            }            
        }

        private List<Person> _persons;

        public List<Person> Persons 
        {
            get
            {
                if (_persons == null)
                {
                    _persons = new List<Person>();
                    _persons.Add(new Person() { Id = 1, GivenName = "Tom", Surname = "Vaughan‏" });
                    _persons.Add(new Person() { Id = 2, GivenName = "Elizabeth", Surname = "Redman" });
                    _persons.Add(new Person() { Id = 3, GivenName = "Mike", Surname = "Rabinovich" });
                    _persons.Add(new Person() { Id = 4, GivenName = "Kate", Surname = "Huber" });
                    _persons.Add(new Person() { Id = 5, GivenName = "Luke", Surname = "Schrodinger" });
                }
                return _persons;
            }            
        }

        private List<Status> _statuses;

        public List<Status> Statuses
        {
            get
            {
                if (_statuses == null)
                {
                    _statuses = new List<Status>();
                    _statuses.Add(new Status() { Id = 1, Name = "New" });
                    _statuses.Add(new Status() { Id = 2, Name = "In Progress" });
                    _statuses.Add(new Status() { Id = 3, Name = "Done" });
                }
                return _statuses;
            }
        }
    }
}
