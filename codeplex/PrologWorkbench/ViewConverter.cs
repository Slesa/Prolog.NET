/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Prolog.Workbench
{
    public sealed class ViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Views valueView = (Views)value;
            Views parameterView;
            string parameterString = parameter as string;
            if (parameterString != null)
            {
                parameterView = (Views)Enum.Parse(typeof(Views), parameterString);
            }
            else
            {
                parameterView = (Views)parameter;
            }

            bool result = valueView == parameterView;

            if (targetType == typeof(Visibility))
            {
                return result ? Visibility.Visible : Visibility.Collapsed;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
