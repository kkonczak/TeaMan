using System;
using System.Globalization;
using System.Windows.Data;

namespace TeaMan.ValueConverters
{
    public class DateToTimeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is DateTime dateValue ?
                dateValue.ToString("H:mm") :
                string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
