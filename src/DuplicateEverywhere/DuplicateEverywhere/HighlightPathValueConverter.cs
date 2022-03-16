using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace DuplicateEverywhere
{
    [ValueConversion(typeof(object), typeof(bool))]
    class HighlightPathValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && Filter.Current.HighlightPaths.Any(p => 
                                    value.ToString().Contains(p, StringComparison.OrdinalIgnoreCase)))
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
