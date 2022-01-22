using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using TeaMan.Interactions;
using TeaMan.Models;

namespace TeaMan.ViewModels
{
    public class AddTaskViewModel : ReactiveObject
    {
        public AddTaskViewModel(Calendar calendar)
        {
            TaskStatuses.AddRange(calendar.TaskStatuses);
            TaskTypes.AddRange(calendar.TaskTypes);

            Model.CalendarId = calendar.Id;
            Model.PlannedStart = DateTime.Now;
            Model.PlannedEnd = DateTime.Now;

            Ok = ReactiveCommand.CreateFromObservable(() => CloseWindowInteraction.CloseWindow.Handle(true));
            Cancel = ReactiveCommand.CreateFromObservable(() => CloseWindowInteraction.CloseWindow.Handle(false));
        }

        [Reactive]
        public string WindowName { get; set; } = "Add Task";

        public ObservableCollection<TaskStatus> TaskStatuses { get; set; } = new ObservableCollection<TaskStatus>();

        public ObservableCollection<TaskType> TaskTypes { get; set; } = new ObservableCollection<TaskType>();

        public UserTask Model { get; set; } = new UserTask();

        public ReactiveCommand<Unit, Unit> Ok { get; }

        public ReactiveCommand<Unit, Unit> Cancel { get; }
    }
}
