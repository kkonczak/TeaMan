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

            AddTaskType = ReactiveCommand.CreateFromTask(AddTaskTypeImpl);
            AddTaskType.ThrownExceptions.Subscribe(ex => { Debug.WriteLine(ex); });

            AddTaskStatus = ReactiveCommand.CreateFromTask(AddTaskStatusImpl);
            AddTaskStatus.ThrownExceptions.Subscribe(ex => { Debug.WriteLine(ex); });

            AddCalendar = ReactiveCommand.CreateFromTask(AddCalendarImpl);
            AddCalendar.ThrownExceptions.Subscribe(ex => { Debug.WriteLine(ex); });

            DeleteCalendar = ReactiveCommand.CreateFromTask<Calendar>(DeleteCalendarImpl);
            DeleteCalendar.ThrownExceptions.Subscribe(ex => { Debug.WriteLine(ex); });

            RefreshShowedTasks = ReactiveCommand.CreateFromTask(RefreshShowedTasksImpl);
            RefreshShowedTasks.ThrownExceptions.Subscribe(ex => { Debug.WriteLine(ex); });

            this.WhenAnyValue(
                x => x.ViewStartDate,
                x => x.ViewEndDate,
                x => x.SelectedTaskStatus,
                x => x.SelectedTaskType,
                x => x.SelectedCalendar)
                .Select(x => Unit.Default)
                .InvokeCommand(RefreshShowedTasks);

            this.WhenAnyObservable(x => x.RefreshShowedTasks.IsExecuting)
                .ToPropertyEx(this, x => x.IsLoading);

            // Test load
            Load.Execute();
        }

        public ReactiveCommand<Unit, Unit> Load { get; }

        public ReactiveCommand<Unit, Unit> AddTask { get; }

        public ReactiveCommand<Unit, Unit> AddCalendar { get; }

        public ReactiveCommand<Unit, Unit> AddTaskType { get; }

        public ReactiveCommand<Unit, Unit> AddTaskStatus { get; }

        public ReactiveCommand<Calendar, Unit> DeleteCalendar { get; }

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
            ShowedTasks.Clear();

            if (SelectedCalendar != null)
            {
                ShowedTasks.AddRange(await DatabaseController.GetUserTasksAsync(SelectedCalendar.Id, SelectedTaskStatus?.Id, SelectedTaskType?.Id, ViewStartDate, ViewEndDate));
            }
        }

        private async System.Threading.Tasks.Task LoadImpl()
        {
            await DatabaseController.InitializeDatabaseAsync();

            // Load
            Calendars.Clear();
            Calendars.AddRange(await DatabaseController.GetCalendarsWithIncludedCollectionsAsync());

            SelectedCalendar = Calendars.FirstOrDefault(e => e.Id == SelectedCalendar?.Id) ?? Calendars[0];

            await RefreshShowedTasksImpl();
        }

        private async System.Threading.Tasks.Task AddTaskImpl()
        {
            var vm = new AddTaskViewModel(SelectedCalendar);

            if (DialogHelper.ShowDialog(vm) == true)
            {
                await DatabaseController.AddUserTaskAsync(vm.Model);

                await RefreshShowedTasksImpl();
            }
        }

        private async System.Threading.Tasks.Task AddCalendarImpl()
        {
            var vm = new AddCalendarViewModel();

            if (DialogHelper.ShowDialog(vm) == true)
            {
                await DatabaseController.AddCalendarAsync(vm.Model);
                await ReloadCalendars();
            }
        }

        private async System.Threading.Tasks.Task AddTaskTypeImpl()
        {
            var vm = new AddTaskTypeViewModel(SelectedCalendar);

            if (DialogHelper.ShowDialog(vm) == true)
            {
                await DatabaseController.AddTaskTypeAsync(vm.Model);
                await ReloadCalendars();
            }
        }

        private async System.Threading.Tasks.Task AddTaskStatusImpl()
        {
            var vm = new AddTaskStatusViewModel(SelectedCalendar);

            if (DialogHelper.ShowDialog(vm) == true)
            {
                await DatabaseController.AddTaskStatusAsync(vm.Model);
                await ReloadCalendars();
            }
        }

        private async System.Threading.Tasks.Task ReloadCalendars()
        {
            var previousCalendarId = SelectedCalendar.Id;
            Calendars.Clear();
            Calendars.AddRange(await DatabaseController.GetCalendarsWithIncludedCollectionsAsync());
            SelectedCalendar = Calendars.FirstOrDefault(e => e.Id == previousCalendarId);
        }

        private async System.Threading.Tasks.Task DeleteCalendarImpl(Calendar calendar)
        {
            if (DialogHelper.ShowMessageBox($"Are you sure you want to delete calendar \"{calendar.Name}\"?", "Calendar delete", "Yes", "No") == true)
            {
                await DatabaseController.DeleteCalendarAsync(calendar);
                await ReloadCalendars();
            }
        }
    }
}
