using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace KOTApp.Converters
{
    public class TableColorConverter:IValueConverter
    {
        static TableColorConverter Instance = new TableColorConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                if (value == null) return false;
                if ((bool)value)
                {
                    return "#7c0000";
                }
                else
                {
                    return "#044c1e";
                }
            }
            catch 
            {
                return Color.Green;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
