using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace KOTApp.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                    return false;
                if (value.ToString() == "0")
                {
                    if(parameter.ToString() == "0")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                    if (parameter.ToString() == "0")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
            }
            catch
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
