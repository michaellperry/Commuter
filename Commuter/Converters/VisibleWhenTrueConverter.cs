using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Commuter.Converters
{
    public class VisibleWhenTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(Visibility) && targetType != typeof(Visibility?))
                throw new InvalidOperationException("VisibleWhenTrueConverter can only be used with Visibility properties.");

            return ObjectToBoolean(value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private static bool ObjectToBoolean(object value)
        {
            if (value != null && value is bool)
                return (bool)value;
            return false;
        }
    }
}
