using System;
using System.Windows;
using System.Windows.Controls;

namespace TeaMan.Controls
{
    public class DateTimePicker : Control
    {
        public static readonly DependencyProperty DateTimeProperty =
            DependencyProperty.Register(nameof(DateTime), typeof(DateTime), typeof(DateTimePicker),
                new PropertyMetadata(default(DateTime), OnDateTimeChanged));

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register(nameof(Date), typeof(DateTime), typeof(DateTimePicker),
                new PropertyMetadata(default(DateTime), OnDateChanged));

        public static readonly DependencyProperty TimeStringProperty =
            DependencyProperty.Register(nameof(TimeString), typeof(string), typeof(DateTimePicker),
                new PropertyMetadata(default(string), OnTimeChanged));

        static DateTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimePicker), new FrameworkPropertyMetadata(typeof(DateTimePicker)));
        }

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public DateTime DateTime
        {
            get { return (DateTime)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); }
        }

        public string TimeString
        {
            get { return (string)GetValue(TimeStringProperty); }
            set { SetValue(TimeStringProperty, value); }
        }

        private static void OnDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((DateTimePicker)d).TimeString = ((DateTime)e.NewValue).ToString("HH:mm");
                ((DateTimePicker)d).Date = ((DateTime)e.NewValue).Date;
            }
        }

        private static void OnTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                if (TimeSpan.TryParse((string)e.NewValue, out TimeSpan timeSpan))
                {
                    ((DateTimePicker)d).DateTime = ((DateTimePicker)d).Date.Date + timeSpan;
                }
                else
                {
                    ((DateTimePicker)d).TimeString = "00:00";
                }
            }
        }

        private static void OnDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                if (TimeSpan.TryParse(((DateTimePicker)d).TimeString, out TimeSpan timeSpan))
                {
                    ((DateTimePicker)d).DateTime = ((DateTime)e.NewValue).Date + timeSpan;
                }
                else
                {
                    ((DateTimePicker)d).TimeString = "00:00";
                }
            }
        }
    }
}
