using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeCore.Extensions
{
    public static class ReflectionExtensions
    {
        public static void SetPropertyValue(this object obj, string propertyName, object value, object[] index = null)
        {
            obj.GetType().GetProperty(propertyName).SetValue(obj, value, index);
        }
    }
}
