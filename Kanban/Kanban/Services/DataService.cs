using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Bogus;
using Kanban.Models;

namespace Kanban.Services
{
    [Export(typeof(IDataService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class DataService : IDataService
    {
        private Faker _faker;
        private Random _rnd;

        public DataService()
        {
            _faker = new Faker("en");
            _rnd = new Random();
        }

        private List<WorkItem> _workItems;

        public List<WorkItem> WorkItems
        {
            get
            {
                if (_workItems == null)
                {
                    _workItems = new List<WorkItem>();
                    for (int i = 1; i < 12; i++)
                    {
                        _workItems.Add(new WorkItem() { Id = _rnd.Next(10, 99), Status = Statuses[_rnd.Next(0, 3)], User = Users[_rnd.Next(0, 5)], Title = _faker.Hacker.Phrase() });
                    }
                }
                return _workItems;
            }
        }

        private List<User> _users;

        public List<User> Users
        {
            get
            {
                if (_users == null)
                {
                    _users = new List<User>();
                    for (int i = 1; i < 6; i++)
                    {
                        _users.Add(new User() { Id = i, GivenName = _faker.Name.FirstName(), Surname = _faker.Name.LastName() });
                    }
                }
                return _users;
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
