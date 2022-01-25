using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TeaMan.Controls.Calendar.Models
{
    public class DayInCalendar
    {
        public int Column { get; set; }

        public int Row { get; set; }

        public string Label { get; set; }

        public int DayInMonth { get; set; }

        public bool IsHeader { get; set; }

        public ObservableCollection<Appointment> Appointments { get; set; } = new ObservableCollection<Appointment>();
    }
}
