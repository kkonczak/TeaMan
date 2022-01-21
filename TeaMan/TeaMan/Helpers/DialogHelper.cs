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
            [typeof(AddCalendarViewModel)] = typeof(AddCalendarView)
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
    }
}
