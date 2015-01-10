using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace StoryTimeDevKit.Converters.ToolbarRadioButtons
{
    public class BooleanToEnumValueConverter<TEnum> : IValueConverter where TEnum : struct 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TEnum)) return false;
            if (!(parameter is TEnum)) return false;
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return false;
            if ((bool)value)
            {
                return parameter;
            }
            return null;
        }
    }
}
