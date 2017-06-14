﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Idealde.Modules.Shell.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Equals(value, true)) return Visibility.Visible;
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Equals(value, Visibility.Visible)) return true;
            return false;
        }
    }
}