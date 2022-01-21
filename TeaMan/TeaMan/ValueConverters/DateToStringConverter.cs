using System;
using System.Globalization;
using System.Windows.Data;

namespace TeaMan.ValueConverters
{
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is DateTime dateValue ?
                dateValue.Date.ToLongDateString() :
                string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
