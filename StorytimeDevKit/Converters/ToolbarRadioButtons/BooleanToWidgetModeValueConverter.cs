using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using StoryTimeDevKit.SceneWidgets.Interfaces;

namespace StoryTimeDevKit.Converters.ToolbarRadioButtons
{
    public class BooleanToWidgetModeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is WidgetMode)) return false;
            if (!(parameter is WidgetMode)) return false;
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
