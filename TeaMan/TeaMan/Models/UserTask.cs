﻿using System;

namespace TeaMan.Models
{
    public class UserTask : Entity
    {
        public string Name { get; set; }

        public int TaskStatusId { get; set; }

        public int TaskTypeId { get; set; }

        public int CalendarId { get; set; }

        public DateTime? PlannedStart { get; set; }

        public DateTime? PlannedEnd { get; set; }

        public TaskStatus TaskStatus { get; set; }

        public TaskType TaskType { get; set; }

        public Calendar Calendar { get; set; }
    }
}
