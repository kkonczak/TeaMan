using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace TeaMan.Controls.Calendar.ValueConverters
{
    public class IsSelectedToBorderBrushConverter : IValueConverter
    {
        public const string AccentColor = "AccentColor";
        public const string BorderColor = "BorderColor";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is bool isSelected && isSelected ?
                Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Contains(AccentColor))[AccentColor] :
                Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Contains(BorderColor))[BorderColor];

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
