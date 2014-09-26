using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StoryTimeUI.DataBinding.Binders
{
    public class PropertyBinder
    {
        private PropertyInfo _destinationProperty;
        private PropertyInfo _sourceProperty;
        private object _destination;
        private object _source;

        public PropertyBinder(
            PropertyInfo destinationProperty, PropertyInfo sourceProperty,
            object destination, object source)
        {
            _destinationProperty = destinationProperty;
            _sourceProperty = sourceProperty;
            _destination = destination;
            _source = source;
        }

        public void Bind()
        {
            _destinationProperty.SetValue(_destination, _sourceProperty.GetValue(_source, null), null);
        }
    }
}
