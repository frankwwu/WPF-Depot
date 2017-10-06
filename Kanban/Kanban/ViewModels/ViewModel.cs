using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Kanban.Models;
using Kanban.Mvvm;
using Kanban.Services;

namespace Kanban.ViewModels
{
    [Export(typeof(ViewModel))]
    public class ViewModel
    {
        private readonly IDataService _dataService;

        [ImportingConstructor]
        public ViewModel(IDataService dataService)
        {
            _dataService = dataService;
            WorkItems = _dataService.WorkItems;
            WorkItemsCV = CollectionViewSource.GetDefaultView(WorkItems);
            WorkItemsCV.GroupDescriptions.Add(new PropertyGroupDescription("Status.Name"));
        }

        #region Public Properties

        public List<WorkItem> WorkItems { get; private set; }

        private ICollectionView _workItemsCV;

        public ICollectionView WorkItemsCV
        {
            get { return _workItemsCV; }
            set
            {
                _workItemsCV = value;
            }
        }

        #endregion Public Properties

        #region EditCommand

        public ICommand EditCommand
        {
            get { return new DelegateCommand<object>(Edit, o => { return true; }); }
        }

        internal void Edit(object parameter)
        {
            MessageBox.Show(parameter.ToString(), "Edit");
        }

        #endregion EditCommand

        #region ContextMenuCommand

        public ICommand ContextMenuCommand
        {
            get { return new DelegateCommand<object>(ContextMenu, o => { return true; }); }
        }

        internal void ContextMenu(object parameter)
        {
            Button btn = parameter as Button;
            btn.ContextMenu.IsEnabled = true;
            btn.ContextMenu.DataContext = this;
            btn.ContextMenu.PlacementTarget = btn;
            btn.ContextMenu.Placement = PlacementMode.Bottom;
            btn.ContextMenu.IsOpen = true;
        }

        #endregion ContextMenuCommand
    }
}
