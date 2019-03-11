using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RestApiTestSolution.View
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var bValue = false;
                var bIsInverse = false;
                if (value is bool b)
                {
                    bValue = b;
                }

                if (parameter is bool p)
                {
                    bIsInverse = p;
                }

                if (bIsInverse)
                {
                    return (bValue) ? Visibility.Collapsed : Visibility.Visible;
                }
                return (bValue) ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (InvalidCastException e)
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
