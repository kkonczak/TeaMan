using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;

namespace TeaMan.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            Command1 = ReactiveCommand.Create(Command1Impl);

            this.WhenAnyValue(e => e.Pole1)
                .Where(e => !string.IsNullOrEmpty(e) && e.Length % 2 == 0)
                .Select(e => Unit.Default)
                .InvokeCommand(Command1);
        }

        [Reactive]
        public string Pole1 { get; set; }

        public ReactiveCommand<Unit, Unit> Command1 { get; }

        private void Command1Impl()
        {
            Debug.WriteLine("test");
        }
    }
}
