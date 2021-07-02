using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;

namespace DataGridColumnBinding
{
    [Export(typeof(ViewModel))]
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly IService _service;

        [ImportingConstructor]
        public ViewModel(IService service)
        {
            _service = service;
            DataGridModel = _service.Refresh();           
        }

        #region Public Properties

        public string Title { get { return "DataGrid Column Binding"; } }

        private DataGridModel _dataGridModel;

        public DataGridModel DataGridModel
        {
            get { return _dataGridModel; }
            set { _dataGridModel = value; NotifyPropertyChanged(); }
        }

        #endregion Public Properties

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
