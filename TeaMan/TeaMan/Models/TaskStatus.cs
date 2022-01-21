using System.Collections.Generic;

namespace TeaMan.Models
{
    public class TaskStatus : OrderedListItemEntity
    {
        public TaskStatus(string name, int order) : base(name, order) { }

        public TaskStatus() { }

        public int CalendarId { get; set; }

        public Calendar Calendar { get; set; }

        public ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
    }
}
