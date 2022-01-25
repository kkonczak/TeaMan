using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows;
using System.Windows.Controls;
using TeaMan.Models;

namespace TeaMan.Controls
{
    /// <summary>
    /// Interaction logic for MyTasksControl.xaml
    /// </summary>
    public partial class MyTasksControl : UserControl
    {
        public static readonly DependencyProperty StartDateProperty =
             DependencyProperty.Register(nameof(StartDate), typeof(DateTime), typeof(MyTasksControl),
                new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty SelectedDateProperty =
             DependencyProperty.Register(nameof(SelectedDate), typeof(DateTime), typeof(MyTasksControl),
                new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty EndDateProperty =
             DependencyProperty.Register(nameof(EndDate), typeof(DateTime), typeof(MyTasksControl),
                new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty UserTasksProperty =
             DependencyProperty.Register(nameof(UserTasks), typeof(ObservableCollection<UserTask>), typeof(MyTasksControl),
                new PropertyMetadata(default));

        public MyTasksControl()
        {
            InitializeComponent();

            PreviousDay = ReactiveCommand.Create(PreviousDayImpl);
            NextDay = ReactiveCommand.Create(NextDayImpl);
        }

        public ReactiveCommand<Unit, Unit> PreviousDay { get; }

        public ReactiveCommand<Unit, Unit> NextDay { get; }

        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public DateTime EndDate
        {
            get { return (DateTime)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

        public ObservableCollection<UserTask> UserTasks
        {
            get { return (ObservableCollection<UserTask>)GetValue(UserTasksProperty); }
            set { SetValue(UserTasksProperty, value); }
        }

        private void PreviousDayImpl()
        {
            StartDate = StartDate.Date.AddDays(-1);
            SelectedDate = StartDate;
            EndDate = StartDate.Date.AddDays(1).AddMilliseconds(-1);
        }

        private void NextDayImpl()
        {
            StartDate = StartDate.Date.AddDays(1);
            SelectedDate = StartDate;
            EndDate = StartDate.Date.AddDays(1).AddMilliseconds(-1);
        }
    }
}
