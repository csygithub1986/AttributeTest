using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AttributeTest
{
    public class EnumToArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type enumType = value as Type;
            return Enum.GetValues(enumType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value is bool)
                {
                    bool reverse = false;
                    bool isOnlyHide = false;
                    bool isVisible = (bool)value;
                    if (parameter != null)
                    {
                        if (parameter.ToString() == "OnlyHide")
                            isOnlyHide = true;
                        bool.TryParse(parameter.ToString(), out reverse);
                    }
                    if (isOnlyHide)
                    {
                        if (isVisible)
                            return Visibility.Visible;
                        return Visibility.Hidden;
                    }
                    if (isVisible)
                        return reverse ? Visibility.Collapsed : Visibility.Visible;
                    return reverse ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            catch (Exception)
            {

            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
