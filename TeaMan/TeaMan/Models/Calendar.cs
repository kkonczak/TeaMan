using System.Collections.Generic;

namespace TeaMan.Models
{
    public class Calendar : OrderedListItemEntity
    {
        public ICollection<UserTask> Tasks { get; set; } = new List<UserTask>();
        public ICollection<TaskStatus> TaskStatuses { get; set; } = new List<TaskStatus>();
        public ICollection<TaskType> TaskTypes { get; set; } = new List<TaskType>();
    }
}
