using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using TeaMan.Models;

namespace TeaMan.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            Load = ReactiveCommand.CreateFromTask(LoadImpl);

            // Test load
            Load.Execute();

            // Test below

            Command1 = ReactiveCommand.Create(Command1Impl);

            this.WhenAnyValue(e => e.Pole1)
                .Where(e => !string.IsNullOrEmpty(e) && e.Length % 2 == 0)
                .Select(e => Unit.Default)
                .InvokeCommand(Command1);
        }

        [Reactive]
        public string Pole1 { get; set; }
        public ReactiveCommand<Unit, Unit> Command1 { get; }


        public ReactiveCommand<Unit, Unit> Load { get; }

        #region Model

        public ObservableCollection<Calendar> Calendars { get; set; } = new ObservableCollection<Calendar>();

        [Reactive]
        public Calendar SelectedCalendar { get; set; }

        #endregion

        private async System.Threading.Tasks.Task LoadImpl()
        {
            var calendar = new Calendar()
            {
                Name = "Calendar 1",
                Order = 1,
                TaskStatuses = new ObservableCollection<TaskStatus>(
                    new TaskStatus[] {
                        new TaskStatus() {Name = "To Do", Order = 1},
                        new TaskStatus() {Name = "In Progress", Order = 2},
                        new TaskStatus() {Name = "Done", Order = 3}
                    }),
                TaskTypes = new ObservableCollection<TaskType>(
                    new TaskType[] {
                        new TaskType() {Name = "Backlog", Order = 1},
                        new TaskType() {Name = "Feature", Order = 2},
                        new TaskType() {Name = "Bug", Order = 3}
                    })
            };

            Calendars.Add(calendar);
        }

        private void Command1Impl()
        {
            Debug.WriteLine("test");
        }
    }
}
