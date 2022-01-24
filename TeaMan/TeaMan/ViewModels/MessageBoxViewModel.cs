using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using TeaMan.Interactions;

namespace TeaMan.ViewModels
{
    public class MessageBoxViewModel : ReactiveObject
    {
        public MessageBoxViewModel(
            string message,
            string title = "TeaMan",
            string okButtonText = "OK",
            string cancelButtonText = "Cancel")
        {
            Message = message;
            WindowName = title;
            OkButtonText = okButtonText;
            CancelButtonText = cancelButtonText;

            Ok = ReactiveCommand.CreateFromObservable(() => CloseWindowInteraction.CloseWindow.Handle(true));
            Cancel = ReactiveCommand.CreateFromObservable(() => CloseWindowInteraction.CloseWindow.Handle(false));
        }

        [Reactive]
        public string WindowName { get; set; }

        [Reactive]
        public string Message { get; set; }

        [Reactive]
        public string OkButtonText { get; set; }

        [Reactive]
        public string CancelButtonText { get; set; }

        public ReactiveCommand<Unit, Unit> Ok { get; }

        public ReactiveCommand<Unit, Unit> Cancel { get; }
    }
}
