using DynamicData;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
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

            // Test load
            Load.Execute();
        }

        public ReactiveCommand<Unit, Unit> Load { get; }

        public ReactiveCommand<Unit, Unit> AddTask { get; }

        #region Model

        public ObservableCollection<Calendar> Calendars { get; set; } = new ObservableCollection<Calendar>();

        [Reactive]
        public Calendar SelectedCalendar { get; set; }

        #endregion

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
                    dbContext.Tasks.Add(vm.Model);
                    await dbContext.SaveChangesAsync();
                }

                Load.Execute();
            }
        }
    }
}
