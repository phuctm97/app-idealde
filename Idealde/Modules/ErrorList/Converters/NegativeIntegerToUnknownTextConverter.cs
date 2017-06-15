using System;
using System.Globalization;
using System.Windows.Data;
using Idealde.Properties;

namespace Idealde.Modules.ErrorList.Converters
{
    public class NegativeIntegerToUnknownTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Resources.UnknownDisplayText;
            int i = (int) value;
            if (i < 0) return Resources.UnknownDisplayText;
            return i;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}