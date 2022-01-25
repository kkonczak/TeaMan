using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using TeaMan.Controls.Calendar.Models;
using TeaMan.Models;

namespace TeaMan.Controls
{
    /// <summary>
    /// Interaction logic for CalendarControl.xaml
    /// </summary>
    public partial class CalendarControl : UserControl
    {
        readonly string[] NamesOfDays = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        public static readonly DependencyProperty StartDateProperty =
             DependencyProperty.Register(
                nameof(StartDate),
                typeof(DateTime),
                typeof(CalendarControl),
                new PropertyMetadata(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0)));

        public static readonly DependencyProperty EndDateProperty =
             DependencyProperty.Register(
                nameof(EndDate),
                typeof(DateTime),
                typeof(CalendarControl),
                new PropertyMetadata(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0).AddMonths(1).AddMilliseconds(-1)));

        public static readonly DependencyProperty UserTasksProperty =
             DependencyProperty.Register(
                nameof(UserTasks),
                typeof(ObservableCollection<UserTask>),
                typeof(CalendarControl),
                new PropertyMetadata(default));

        private Dictionary<int, DayInCalendar> daysInCalendarDictionary = new Dictionary<int, DayInCalendar>();

        public CalendarControl()
        {
            InitializeComponent();

            PreviousMonth = ReactiveCommand.Create(PreviousMonthImpl);
            NextMonth = ReactiveCommand.Create(NextMonthImpl);
            RegenerateCalendar = ReactiveCommand.Create(RegenerateCalendarImpl);
            UpdateAppointments = ReactiveCommand.Create(UpdateAppointmentsImpl);

            this.WhenAnyValue(x => x.StartDate)
                .Select(x => Unit.Default)
                .InvokeCommand(RegenerateCalendar);

            this.WhenAnyValue(x => x.UserTasks)
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    Observable.FromEventPattern(x, nameof(x.CollectionChanged))
                        .Select(x => Unit.Default)
                        .InvokeCommand(UpdateAppointments);
                });
        }

        private ReactiveCommand<Unit, Unit> RegenerateCalendar { get; }

        private ReactiveCommand<Unit, Unit> UpdateAppointments { get; }

        public ReactiveCommand<Unit, Unit> PreviousMonth { get; }

        public ReactiveCommand<Unit, Unit> NextMonth { get; }

        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        public DateTime EndDate
        {
            get { return (DateTime)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

        public ObservableCollection<UserTask> UserTasks
        {
            get { return (ObservableCollection<UserTask>)GetValue(UserTasksProperty); }
            set { SetValue(UserTasksProperty, value); }
        }

        public ObservableCollection<DayInCalendar> DaysInCalendar { get; set; } = new ObservableCollection<DayInCalendar>();

        private void PreviousMonthImpl()
        {
            var firstDateOfCurrentMonth = new DateTime(StartDate.Year, StartDate.Month, 1, 0, 0, 0);
            StartDate = firstDateOfCurrentMonth.AddMonths(-1);
            EndDate = StartDate.Date.AddMonths(1).AddMilliseconds(-1);
        }

        private void NextMonthImpl()
        {
            var firstDateOfCurrentMonth = new DateTime(StartDate.Year, StartDate.Month, 1, 0, 0, 0);
            StartDate = firstDateOfCurrentMonth.AddMonths(1);
            EndDate = StartDate.Date.AddMonths(1).AddMilliseconds(-1);
        }

        private void RegenerateCalendarImpl()
        {
            DaysInCalendar.Clear();
            daysInCalendarDictionary.Clear();
            var dayOfWeek = ((int)StartDate.DayOfWeek + 6) % 7;
            var dayOfMonth = 1;
            var lastDayOfMonth = GetDaysNumberForMonthAndYear(StartDate.Month, StartDate.Year);

            // Create headers
            for (int x = 0; x < NamesOfDays.Length; x++)
            {
                DaysInCalendar.Add(new DayInCalendar()
                {
                    Label = NamesOfDays[x],
                    Column = x,
                    Row = 0,
                    IsHeader = true
                });
            }

            // Create content
            for (var y = 0; y <= 5 && dayOfMonth <= lastDayOfMonth; y++)
            {
                for (var x = (y == 0 ? dayOfWeek : 0); x <= 6 && dayOfMonth <= lastDayOfMonth; x++)
                {
                    var dayInCalendar = new DayInCalendar()
                    {
                        Label = dayOfMonth.ToString(),
                        DayInMonth = dayOfMonth,
                        Column = x,
                        Row = y + 1
                    };

                    DaysInCalendar.Add(dayInCalendar);
                    daysInCalendarDictionary.Add(dayOfMonth, dayInCalendar);

                    dayOfMonth++;
                }
            }
        }

        private void UpdateAppointmentsImpl()
        {
            if (UserTasks == null)
            {
                return;
            }

            foreach (var dayInCalendar in DaysInCalendar)
            {
                dayInCalendar.Appointments?.Clear();
            }

            foreach (var userTask in UserTasks)
            {
                var firstDayOfTask = !userTask.PlannedStart.HasValue || userTask.PlannedStart.Value < StartDate ?
                    1 :
                    userTask.PlannedStart.Value.Day;
                var lastDayOfTask = !userTask.PlannedEnd.HasValue || userTask.PlannedEnd.Value > EndDate ?
                    GetDaysNumberForMonthAndYear(StartDate.Month, StartDate.Year) :
                    userTask.PlannedEnd.Value.Day;

                foreach (var day in Enumerable.Range(firstDayOfTask, lastDayOfTask - firstDayOfTask + 1))
                {
                    daysInCalendarDictionary[day].Appointments.Add(new Appointment { Name = userTask.Name });
                }
            }
        }

        private int GetDaysNumberForMonthAndYear(int month, int year) =>
            month switch
            {
                9 => 30,
                4 => 30,
                6 => 30,
                11 => 30,
                2 => (year % 4 == 0 && year % 100 != 0) || year % 400 == 0 ? 29 : 28,
                _ => 31
            };
    }
}
