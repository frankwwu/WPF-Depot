using Bogus;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;

namespace ScrollToSelectedItem
{
    public class ViewModel: INotifyPropertyChanged
    {
        private Faker _faker;

        public ViewModel()
        {
            _faker = new Faker("en");

            List<User> names = GenerateRandomUsers(25);
            Users = new ObservableCollection<User>(names);

            UsersCV = CollectionViewSource.GetDefaultView(Users);
            UsersCV.SortDescriptions.Add(new SortDescription("FullName", ListSortDirection.Ascending));
        }

        #region Public Properties

        private ObservableCollection<User> _users;

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged();
                if ((_users != null) && (_users.Count > 0))
                {
                    SelectedUser = _users.FirstOrDefault();
                }
            }
        }

        private ICollectionView _usersCV;

        public ICollectionView UsersCV
        {
            get => _usersCV;
            set { _usersCV = value; OnPropertyChanged(); }
        }
        private User _selectedUser;

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        #endregion Public Properties

        private List<User> GenerateRandomUsers(int numberOfEntries)
        {
            List<User> namesAndEmails = new List<User>();

            for (int i = 0; i < numberOfEntries; i++)
            {
                User user = new User();
                user.FullName = _faker.Name.FullName();
                user.Email = _faker.Internet.Email(user.FullName);
                namesAndEmails.Add(user);
            }

            return namesAndEmails;
        }

        #region AddCommand

        public ICommand AddCommand
        {
            get { return new DelegateCommand<object>(Add, o => { return true; }); }
        }

        internal void Add(object parameter)
        {
            User user = new User();
            user.FullName = _faker.Name.FullName();
            user.Email = _faker.Internet.Email(user.FullName);
            Users.Add(user);

            SelectedUser = Users.FirstOrDefault(u => u.FullName.Equals(user.FullName));
        }

        #endregion AddCommand

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
