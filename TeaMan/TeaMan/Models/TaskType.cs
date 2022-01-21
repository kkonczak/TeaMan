using System.Collections.Generic;

namespace TeaMan.Models
{
    public class TaskType : OrderedListItemEntity
    {
        public TaskType(string name, int order) : base(name, order) { }

        public TaskType() { }

        public int CalendarId { get; set; }

        public Calendar Calendar { get; set; }

        public ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
    }
}
