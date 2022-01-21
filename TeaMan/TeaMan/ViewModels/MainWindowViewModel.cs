using DynamicData;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using TeaMan.Helpers;
using TeaMan.Models;

namespace TeaMan.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            Load = ReactiveCommand.CreateFromTask(LoadImpl);
            Load.ThrownExceptions.Subscribe(ex => { Debug.WriteLine(ex); });

            AddTask = ReactiveCommand.CreateFromTask(AddTaskImpl);
            AddTask.ThrownExceptions.Subscribe(ex => { Debug.WriteLine(ex); });

            RefreshShowedTasks = ReactiveCommand.CreateFromTask(RefreshShowedTasksImpl);
            RefreshShowedTasks.ThrownExceptions.Subscribe(ex => { Debug.WriteLine(ex); });

            this.WhenAnyValue(
                x => x.ViewStartDate,
                x => x.ViewEndDate,
                x => x.SelectedTaskStatus,
                x => x.SelectedTaskType)
                .Select(x => Unit.Default)
                .InvokeCommand(RefreshShowedTasks);

            this.WhenAnyObservable(x => x.RefreshShowedTasks.IsExecuting)
                .ToPropertyEx(this, x => x.IsLoading);

            // Test load
            Load.Execute();
        }

        public ReactiveCommand<Unit, Unit> Load { get; }

        public ReactiveCommand<Unit, Unit> AddTask { get; }

        public ReactiveCommand<Unit, Unit> RefreshShowedTasks { get; }

        #region Model

        public ObservableCollection<Calendar> Calendars { get; set; } = new ObservableCollection<Calendar>();

        [Reactive]
        public Calendar SelectedCalendar { get; set; }

        [Reactive]
        public TaskType SelectedTaskType { get; set; }

        [Reactive]
        public TaskStatus SelectedTaskStatus { get; set; }

        [Reactive]
        public DateTime ViewStartDate { get; set; } = DateTime.Now.Date;

        [Reactive]
        public DateTime ViewEndDate { get; set; } = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);

        public extern bool IsLoading { [ObservableAsProperty] get; }

        public ObservableCollection<UserTask> ShowedTasks { get; } = new ObservableCollection<UserTask>();

        #endregion

        private async System.Threading.Tasks.Task RefreshShowedTasksImpl()
        {
            using (var dbContext = new DatabaseContext())
            {
                var selectedTaskStatusId = SelectedTaskStatus?.Id;
                var selectedTaskTypeId = SelectedTaskType?.Id;

                var userTasks = await dbContext.UserTasks.Where(e =>
                    (!e.PlannedStart.HasValue || (e.PlannedStart.Value >= ViewStartDate && e.PlannedStart <= ViewEndDate)) &&
                    (!selectedTaskStatusId.HasValue || e.TaskStatusId == selectedTaskStatusId) &&
                    (!selectedTaskTypeId.HasValue || e.TaskTypeId == selectedTaskTypeId))
                    .ToListAsync();

                ShowedTasks.Clear();
                ShowedTasks.AddRange(userTasks);
            }
        }

        private async System.Threading.Tasks.Task LoadImpl()
        {
            using (var dbContext = new DatabaseContext())
            {
                await dbContext.Database.MigrateAsync();
                if (dbContext.Calendars.AsQueryable().Count() == 0)
                {
                    var calendar = new Calendar()
                    {
                        Name = "Calendar 1",
                        Order = 1,
                        TaskStatuses = new ObservableCollection<TaskStatus>(
                            new TaskStatus[] {
                                new TaskStatus() {Name = "In Progress", Order = 2},
                                new TaskStatus() {Name = "To Do", Order = 1},
                                new TaskStatus() {Name = "Done", Order = 3}
                            }),
                        TaskTypes = new ObservableCollection<TaskType>(
                            new TaskType[] {
                                new TaskType() {Name = "Bug", Order = 3},
                                new TaskType() {Name = "Backlog", Order = 1},
                                new TaskType() {Name = "Feature", Order = 2},
                            })
                    };

                    dbContext.Calendars.Add(calendar);
                    await dbContext.SaveChangesAsync();

                    dbContext.UserTasks.Add(new UserTask() { CalendarId = calendar.Id, Name = "TestTask1", PlannedStart = DateTime.Now, TaskStatusId = 1, TaskTypeId = 2 });
                    await dbContext.SaveChangesAsync();
                }

                // Load
                Calendars.Clear();
                Calendars.AddRange(
                    await dbContext.Calendars.AsQueryable()
                        .Include(e => e.TaskStatuses)
                        .Include(e => e.TaskTypes)
                        .ToListAsync());
            }

            SelectedCalendar = Calendars.FirstOrDefault(e => e.Id == SelectedCalendar?.Id) ?? Calendars[0];
        }

        private async System.Threading.Tasks.Task AddTaskImpl()
        {
            var vm = new AddTaskViewModel(SelectedCalendar);

            if (DialogHelper.ShowDialog(vm) == true)
            {
                using (var dbContext = new DatabaseContext())
                {
                    dbContext.UserTasks.Add(vm.Model);
                    await dbContext.SaveChangesAsync();
                }

                await Load.Execute();
            }
        }
    }
}
