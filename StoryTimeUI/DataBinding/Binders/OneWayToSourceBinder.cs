using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace StoryTimeUI.DataBinding.Binders
{
    public class OneWayToSourceBinder : IBinder
    {
        private PropertyInfo _destinationProperty; 
        private PropertyInfo _sourceProperty;
        private object _destination;
        private object _source;

        public OneWayToSourceBinder(
            PropertyInfo destinationProperty, PropertyInfo sourceProperty,
            object destination, object source)
        {
            _destinationProperty = destinationProperty;
            _sourceProperty = sourceProperty;
            _destination = destination;
            _source = source;
        }

        public void Bind(BinderTrigger trigger)
        {
            if (trigger == BinderTrigger.Source) return;
            _sourceProperty.SetValue(_source, _destinationProperty.GetValue(_destination, null), null);
        }
    }
}
