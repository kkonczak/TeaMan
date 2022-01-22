using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TeaMan.Controls
{
    public class CustomComboBox : ComboBox
    {
        public static readonly DependencyProperty AddButtonCommandProperty =
            DependencyProperty.Register(nameof(AddButtonCommand), typeof(ICommand), typeof(CustomComboBox),
                new PropertyMetadata(default));

        public ICommand AddButtonCommand
        {
            get { return (ICommand)GetValue(AddButtonCommandProperty); }
            set { SetValue(AddButtonCommandProperty, value); }
        }
    }
}
