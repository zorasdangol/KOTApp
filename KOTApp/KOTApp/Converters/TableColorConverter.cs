using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace KOTApp.Converters
{
    public class TableColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                if (value == null) return false;
                if ((bool)value)
                {
                    return "Red";
                }
                else
                {
                    return "Green";
                }
            }
            catch 
            {
                return "Green";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
