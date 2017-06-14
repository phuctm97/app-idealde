#region Using Namespace

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#endregion

namespace Idealde.Modules.MainMenu.Converters
{
    public class EmptyStringToUnsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(value as string))
            {
                return DependencyProperty.UnsetValue;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}