using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StoryTimeUI.Exceptions.DataBinding
{
    public class PropertyIsNotWritableException : Exception
    {
        public PropertyInfo PropertyInfo { get; private set; }

        public PropertyIsNotWritableException(PropertyInfo propInfo)
        {
            PropertyInfo = propInfo;
        }
    }
}
