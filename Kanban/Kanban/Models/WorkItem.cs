using System.ComponentModel;

namespace Kanban.Models
{
    public class WorkItem
    {
        public int Id { get; set; }

        public string Title { get; set; }
  
        public Status Status { get; set; }

        public Person Person { get; set; }
    }
}
