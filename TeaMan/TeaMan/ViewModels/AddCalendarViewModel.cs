using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using TeaMan.Interactions;
using TeaMan.Models;

namespace TeaMan.ViewModels
{
    public class AddCalendarViewModel : ReactiveObject
    {
        public AddCalendarViewModel()
        {
            Ok = ReactiveCommand.CreateFromObservable(() => CloseWindowInteraction.CloseWindow.Handle(true));
            Cancel = ReactiveCommand.CreateFromObservable(() => CloseWindowInteraction.CloseWindow.Handle(false));
        }

        public ReactiveCommand<Unit, Unit> Ok { get; }

        public ReactiveCommand<Unit, Unit> Cancel { get; }

        [Reactive]
        public string WindowName { get; set; } = "Add Calendar";

        public Calendar Model { get; set; } = new Calendar();
    }
}
