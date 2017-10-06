using System.Collections.Generic;
using Kanban.Models;

namespace Kanban.Services
{
    public interface IDataService
    {
        List<WorkItem> WorkItems { get; }
        List<Status> Statuses { get; }              
    }
}

