using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace RestApiTestSolution.ViewModel
{
    public class HttpMethodColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((string) value)
            {
                case "GET": return new SolidColorBrush(Colors.GreenYellow);
                case "POST": return new SolidColorBrush(Colors.DeepSkyBlue);
                    default: return new SolidColorBrush(Colors.White); 
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
