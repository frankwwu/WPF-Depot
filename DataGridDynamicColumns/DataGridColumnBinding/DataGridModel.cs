using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace DataGridColumnBinding
{
    public class DataGridModel : INotifyPropertyChanged
    {
        public DataGridModel()
        {
            DataTable = new DataTable();
            ColumnCollection = new Collection<DataGridColumn>();
        }

        #region Public Properties

        public DataTable DataTable { get; set; }

        private Collection<DataGridColumn> _columnCollection;

        public Collection<DataGridColumn> ColumnCollection
        {
            get { return _columnCollection; }
            set { _columnCollection = value; }
        }

        public int CurrentRowId { get; set; }

        public int CurrentColumnId { get; set; }

        public string Header { get; set; }


        private DataGridCellInfo _currentCell;

        public DataGridCellInfo CurrentCell
        {
            get { return _currentCell; }
            set
            {
                _currentCell = value;
                if (_currentCell.Column != null)
                {
                    DataRow row = (_currentCell.Item as System.Data.DataRowView).Row;
                    CurrentRowId = this.DataTable.Rows.IndexOf(row);
                    CurrentColumnId = _currentCell.Column.DisplayIndex;
                    Header = _currentCell.Column.Header as string;
                }
                NotifyPropertyChanged();
            }
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