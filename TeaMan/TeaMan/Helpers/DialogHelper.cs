using System;
using System.Collections.Generic;
using System.Windows;
using TeaMan.ViewModels;
using TeaMan.Views;

namespace TeaMan.Helpers
{
    public static class DialogHelper
    {
        private static Dictionary<Type, Type> mapping = new Dictionary<Type, Type>()
        {
            [typeof(AddTaskViewModel)] = typeof(AddTaskView),
            [typeof(AddCalendarViewModel)] = typeof(AddCalendarView),
            [typeof(AddTaskTypeViewModel)] = typeof(AddTaskTypeView),
            [typeof(AddTaskStatusViewModel)] = typeof(AddTaskStatusView),
            [typeof(MessageBoxViewModel)] = typeof(MessageBoxView),
            [typeof(AddTaskViewModel)] = typeof(AddTaskView)
        };

        public static bool? ShowDialog<T>(T viewModel)
        {
            if (!mapping.ContainsKey(typeof(T)))
            {
                throw new NotImplementedException($"View {typeof(T).Name} not found");
            }

            var view = Activator.CreateInstance(mapping[typeof(T)]);

            if (view is Window viewAsWindow)
            {
                viewAsWindow.DataContext = viewModel;
                viewAsWindow.ShowDialog();
                return viewAsWindow.DialogResult;
            }

            return null;
        }

        public static bool? ShowMessageBox(string message, string title = "TeaMan", string okButtonText = "Ok", string cancelButtonText = "Cancel") =>
            ShowDialog(new MessageBoxViewModel(message, title, okButtonText, cancelButtonText));
    }
}
