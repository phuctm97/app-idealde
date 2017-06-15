using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Idealde.Modules.ErrorList.Converters
{
    public class ErrorItemTypeToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return DependencyProperty.UnsetValue;

            var type = (ErrorListItemType) value;
            switch (type)
            {
                case ErrorListItemType.Error:
                    return new Uri("pack://application:,,,/Idealde;component/Resources/Images/StatusAnnotationCritical.png");
                case ErrorListItemType.Message:
                    return new Uri("pack://application:,,,/Idealde;component/Resources/Images/StatusAnnotationInformation.png");
                case ErrorListItemType.Warning:
                    return new Uri("pack://application:,,,/Idealde;component/Resources/Images/StatusAnnotationWarning.png");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}