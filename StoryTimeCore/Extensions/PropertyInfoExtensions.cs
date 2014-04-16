using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StoryTimeCore.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool ContainsAttribute(this PropertyInfo pi, Type attributeType)
        {
            object[] attributes = Attribute.GetCustomAttributes(pi, attributeType, true);
            return attributes.Length != 0;
        }

        public static bool CanBeGettedAndSetted(this PropertyInfo pi)
        {
            if (pi.GetGetMethod() == null)
                return false;

            if (pi.GetSetMethod() == null)
                return false;

            return true;
        }
    }
}
