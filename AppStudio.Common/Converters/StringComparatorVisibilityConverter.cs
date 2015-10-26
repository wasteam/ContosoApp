using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AppStudio.Common.Converters
{
    public class StringComparatorVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || parameter == null)
            {
                return Visibility.Collapsed;
            }
            else
            {
                string stringValue = value.ToString();
                string stringParameter = parameter.ToString();
                if (stringValue.Equals(stringParameter))
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
