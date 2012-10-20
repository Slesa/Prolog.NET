/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Prolog.Workbench
{
    public sealed class ApplicationTitleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var fileName = (string)values[0];
                var isModified = (bool)values[1];

                fileName = string.IsNullOrEmpty(fileName) ? "Untitled" : Path.GetFileName(fileName);

                return string.Format("{0} - {1}{2}",
                    App.Current.ApplicationTitle,
                    fileName,
                    isModified ? "*" : "");
            }
            catch
            {
                // Handle exceptions when converting during design-time.
                //
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
