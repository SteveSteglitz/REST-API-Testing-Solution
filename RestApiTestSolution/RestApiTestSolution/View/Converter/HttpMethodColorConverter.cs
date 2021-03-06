﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RestApiTestSolution.View.Converter
{
    public class HttpMethodColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((string) value)
            {
                case "GET": return new SolidColorBrush(Colors.GreenYellow);
                case "POST": return new SolidColorBrush(Colors.DeepSkyBlue);
                case "PUT": return new SolidColorBrush(Colors.Khaki);
                case "DELETE": return new SolidColorBrush(Colors.Coral);
                    default: return new SolidColorBrush(Colors.DarkSlateGray); 
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
