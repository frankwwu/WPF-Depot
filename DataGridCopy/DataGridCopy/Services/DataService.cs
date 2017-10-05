using System.Collections.Generic;
using System.ComponentModel.Composition;
using DataGridCopy.Models;

namespace DataGridCopy.Services
{
    [Export(typeof(IDataService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class DataService : IDataService
    {
        public DataService()
        {
        }

        private List<Person> _persons;

        public List<Person> Persons
        {
            get
            {
                if (_persons == null)
                {
                    _persons = new List<Person>();
                    _persons.Add(new Person() { GivenName = "David", Surname = "Knight‏", Address = "17 Morena Blvd", City = "Camarillo", State = "CA", Zip = "93012" });
                    _persons.Add(new Person() { GivenName = "Robin", Surname = "Rabinovich", Address = "386 9th Ave N", City = "Conroe", State = "TX", Zip = "77301" });
                    _persons.Add(new Person() { GivenName = "Christopher", Surname = "De Jesus", Address = "1 Central Ave", City = "Stevens Point", State = "WI", Zip = "54481" });
                    _persons.Add(new Person() { GivenName = "Kate", Surname = "Kuroda", Address = "9 Murfreesboro Rd", City = "Chicago", State = "IL", Zip = "60623" });
                    _persons.Add(new Person() { GivenName = "Thomas", Surname = "Vaughan‏", Address = "6649 N Blue Gum St", City = "New Orleans", State = "LA", Zip = "70116" });
                    _persons.Add(new Person() { GivenName = "Elizabeth", Surname = "Redman", Address = "74 S Westgate St", City = "Albany", State = "NY", Zip = "12204" });
                }
                return _persons;
            }
        }
    }
}
