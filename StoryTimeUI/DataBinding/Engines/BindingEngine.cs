using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using StoryTimeUI.DataBinding.Binders;
using StoryTimeUI.Exceptions.DataBinding;

namespace StoryTimeUI.DataBinding.Engines
{
    [Flags]
    public enum BindingType
    {
        OneWayToDestination = 1 << 1,
        OneWayToSource = 1 << 2,
        TwoWays = OneWayToDestination | OneWayToSource
    }

    public class BindingEngine
    {
        private readonly object _destination;
        private readonly object _source;
        //first PropertyInfo is for destination, second is for source
        private readonly Dictionary<string, List<PropertyBinder>> _sourcePropertyMapping;
        private readonly Dictionary<string, List<PropertyBinder>> _destinationPropertyMapping;

        public BindingEngine(object destination, object source)
        {
            _destination = destination;
            _source = source;

            _sourcePropertyMapping = new Dictionary<string, List<PropertyBinder>>();
            _destinationPropertyMapping = new Dictionary<string,  List<PropertyBinder>>();

            //listen for INotifyPropertyChanged event on the source
            INotifyPropertyChanged sourceNotifiable = source as INotifyPropertyChanged;
            if(sourceNotifiable != null)
                sourceNotifiable.PropertyChanged += SourcePropertyChanged;

            INotifyPropertyChanged destinationNotifiable = destination as INotifyPropertyChanged;
            if (destinationNotifiable != null)
                destinationNotifiable.PropertyChanged += DestinationPropertyChanged;
        }

        public BindingEngine Bind(
            string destinationProperty,
            string sourceProperty,
            BindingType bindingType = BindingType.OneWayToDestination)
        {
            PropertyInfo destinationPropInfo = _destination.GetType().GetProperty(destinationProperty);
            PropertyInfo sourcePropInfo = _source.GetType().GetProperty(sourceProperty);

            if (destinationPropInfo == null || sourcePropInfo == null) return this;
            if (destinationPropInfo.PropertyType != sourcePropInfo.PropertyType) return this;

            AssignBindersWith(destinationPropInfo, sourcePropInfo, bindingType);

            return this;
        }

        protected void AssignBindersWith(PropertyInfo destinationPropInfo, PropertyInfo sourcePropInfo, BindingType bindingType)
        {
            ValidateBinding(destinationPropInfo, sourcePropInfo, bindingType);

            if ((bindingType & BindingType.OneWayToDestination) == BindingType.OneWayToDestination)
            {
                GetSourceBindersFor(sourcePropInfo.Name)
                    .Add(new PropertyBinder(destinationPropInfo, sourcePropInfo, _destination, _source));
            }

            if ((bindingType & BindingType.OneWayToSource) == BindingType.OneWayToSource)
            {
                GetDestinationBindersFor(destinationPropInfo.Name)
                    .Add(new PropertyBinder(sourcePropInfo, destinationPropInfo, _source, _destination));
            }

            if (bindingType == BindingType.OneWayToSource)
                RunDestinationBindingFor(destinationPropInfo.Name);
            else
                RunSourceBindingFor(sourcePropInfo.Name);
        }

        protected void RunSourceBindingFor(string propertyName)
        {
            if (_sourcePropertyMapping.ContainsKey(propertyName))
            {
                List<PropertyBinder> binders = _sourcePropertyMapping[propertyName];
                foreach(PropertyBinder binder in binders)
                    binder.Bind();
            }
        }

        protected void RunDestinationBindingFor(string propertyName)
        {
            if (_destinationPropertyMapping.ContainsKey(propertyName))
            {
                List<PropertyBinder> binders = _destinationPropertyMapping[propertyName];
                foreach(PropertyBinder binder in binders)
                    binder.Bind();
            }
        }

        private void SourcePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            RunSourceBindingFor(args.PropertyName);
        }

        private void DestinationPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            RunDestinationBindingFor(args.PropertyName);
        }

        private void ValidateBinding(PropertyInfo destinationPropInfo, PropertyInfo sourcePropInfo, BindingType bindingType)
        {
            if ((bindingType & BindingType.OneWayToDestination) == BindingType.OneWayToDestination)
            {
                if (!sourcePropInfo.CanRead)
                    throw new PropertyIsNotReadableException(sourcePropInfo);
                if (!destinationPropInfo.CanWrite)
                    throw new PropertyIsNotWritableException(destinationPropInfo);
            }

            if ((bindingType & BindingType.OneWayToSource) == BindingType.OneWayToSource)
            {
                if (!destinationPropInfo.CanRead)
                    throw new PropertyIsNotReadableException(destinationPropInfo);
                if (!sourcePropInfo.CanWrite)
                    throw new PropertyIsNotWritableException(sourcePropInfo);
            }
        }

        private List<PropertyBinder> GetSourceBindersFor(string property)
        {
            return GetBindersFor(property, _sourcePropertyMapping);
        }

        private List<PropertyBinder> GetDestinationBindersFor(string property)
        {
            return GetBindersFor(property, _destinationPropertyMapping);
        }

        private List<PropertyBinder> GetBindersFor(string property, Dictionary<string, List<PropertyBinder>> bindersMap)
        {
            List<PropertyBinder> binders = null;

            if (bindersMap.ContainsKey(property))
            {
                binders = bindersMap[property];
            }
            else
            {
                binders = new List<PropertyBinder>();
                bindersMap.Add(property, binders);
            }

            return binders;
        }
    }
}
