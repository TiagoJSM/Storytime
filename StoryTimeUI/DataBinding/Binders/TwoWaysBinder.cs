using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StoryTimeUI.DataBinding.Binders
{
    public class TwoWaysBinder : IBinder
    {
        private OneWayToSourceBinder _toSourceBinder;
        private OneWayToDestinationBinder _toDestinationBinder;

        public TwoWaysBinder(
            PropertyInfo destinationProperty, PropertyInfo sourceProperty,
            object destination, object source)
        {
            _toSourceBinder = new OneWayToSourceBinder(
                    destinationProperty, sourceProperty,
                    destination, source
                );

            _toDestinationBinder = new OneWayToDestinationBinder(
                    destinationProperty, sourceProperty,
                    destination, source
                );
        }
    
        public void Bind(BinderTrigger trigger)
        {
            if (trigger == BinderTrigger.Destination)
                _toDestinationBinder.Bind(trigger);
            else
                _toSourceBinder.Bind(trigger);
        }
    }
}
