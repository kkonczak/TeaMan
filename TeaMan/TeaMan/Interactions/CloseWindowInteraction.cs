using ReactiveUI;
using System.Reactive;

namespace TeaMan.Interactions
{
    public static class CloseWindowInteraction
    {
        public static Interaction<bool?, Unit> CloseWindow { get; set; } = new Interaction<bool?, Unit>();
    }
}
