using System;
using System.Globalization;
using Xamarin.Forms;

namespace MicrosoftDemoProject
{
    public class NullToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool invert = false;
            if (parameter != null && parameter is string param)
            {
                if (param == "invert")
                {
                    invert = true;
                }
            }

            return !(value == null) ^ invert;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
