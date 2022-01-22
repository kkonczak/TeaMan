using Microsoft.VisualStudio.PlatformUI;
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

        private static readonly DependencyPropertyKey ClearButtonCommandPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ClearButtonCommand), typeof(ICommand), typeof(CustomComboBox),
                new FrameworkPropertyMetadata(default));

        public static readonly DependencyProperty ClearButtonCommandProperty = ClearButtonCommandPropertyKey.DependencyProperty;

        public ICommand AddButtonCommand
        {
            get { return (ICommand)GetValue(AddButtonCommandProperty); }
            set { SetValue(AddButtonCommandProperty, value); }
        }

        public ICommand ClearButtonCommand
        {
            get { return (ICommand)GetValue(ClearButtonCommandProperty); }
            protected set { SetValue(ClearButtonCommandPropertyKey, value); }
        }

        public CustomComboBox()
            : base()
        {
            ClearButtonCommand = new DelegateCommand(() =>
                {
                    SelectedItem = null;
                    IsDropDownOpen = false;
                });
        }
    }
}
