using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StoryTimeUI.Exceptions.DataBinding
{
    public class PropertyIsNotReadableException : Exception
    {
        public PropertyInfo PropertyInfo { get; private set; }

        public PropertyIsNotReadableException(PropertyInfo propInfo)
        {
            PropertyInfo = propInfo;
        }
    }
}
