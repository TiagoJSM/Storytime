using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.ComponentModel;
using StoryTimeFramework.Entities.Actors;
using StoryTimeCore.Physics;
using StoryTimeCore.Resources.Graphic;
using StoryTimeCore.CustomAttributes.Editor;
using StoryTimeCore.Extensions;
using System.Reflection;
using StoryTimeDevKit.Texts;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace StoryTimeDevKit.Models
{
    
    public class PropertyEditorModel : DynamicObject, ICustomTypeDescriptor, INotifyPropertyChanged
    {
        private readonly IDictionary<string, EditablePropertyModel> _dynamicProperties =
            new Dictionary<string, EditablePropertyModel>();
        private object _data;
        private bool _updateSourceOnPropertyChanged;

        public PropertyEditorModel(object data, bool updateSourceOnPropertyChanged = false)
        {
            _data = data;
            _updateSourceOnPropertyChanged = updateSourceOnPropertyChanged;
            AddEditablePropertiesFromData();
            PropertyChanged += OnPropertyChangedHandler;
        }

        private void AddEditablePropertiesFromData()
        {
            var editableProps =
                _data.GetType()
                .GetProperties()
                .Where(prop => prop.ContainsAttribute(typeof(EditableAttribute)))
                .Where(prop => prop.CanBeGettedAndSetted())
                .Select(prop => ConvertToEditableProperty(prop))
                .ToList();

            foreach (var editableProp in editableProps)
                AddProperty(editableProp);
        }

        private EditablePropertyModel ConvertToEditableProperty(PropertyInfo prop)
        {
            var editable = 
                (EditableAttribute)(Attribute.GetCustomAttributes(prop, typeof(EditableAttribute), true).First());

            var propertyName = prop.Name;
            var propGroup = editable.EditorGroup;
            var displayName = editable.EditorName;

            if (String.IsNullOrWhiteSpace(propGroup))
                propGroup = ActorPropertyEditorDefaultValues.DefaultGroupName;

            if (String.IsNullOrWhiteSpace(displayName))
                displayName = propertyName;
            
            return new EditablePropertyModel()
            {
                PropertyGroup = propGroup,
                PropertyName = propertyName,
                DisplayName = displayName,
                Data = prop.GetValue(_data, null),
                DataType = prop.PropertyType
            };
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var memberName = binder.Name;
            EditablePropertyModel bucket;
            var gotValue = _dynamicProperties.TryGetValue(memberName, out bucket);
            result = bucket.Data;
            return gotValue;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var memberName = binder.Name;
            var model = new EditablePropertyModel() 
            {
                Data = value,
                DataType = binder.ReturnType,
                PropertyName = memberName,
                PropertyGroup = ActorPropertyEditorDefaultValues.DefaultGroupName
            };
            AddProperty(model);
            return true;
        }

        protected void AddProperty(EditablePropertyModel editableProp)
        {
            _dynamicProperties[editableProp.PropertyName] = editableProp;
            NotifyToRefreshAllProperties();
        }


        #region Implementation of ICustomTypeDescriptor

        public PropertyDescriptorCollection GetProperties()
        {
            // of course, here must be the attributes associated
            // with each of the dynamic properties

            var properties = _dynamicProperties
                .Select(pair => new DynamicPropertyDescriptor(
                    this,
                    pair.Value.DisplayName,
                    pair.Value.PropertyName, 
                    pair.Value.DataType, 
                    GetPropertyEditorAttributesFrom(pair.Value)));
            return new PropertyDescriptorCollection(properties.ToArray());
        }

        private Attribute[] GetPropertyEditorAttributesFrom(EditablePropertyModel model)
        {
            if (IsExpandable(model))
                return new Attribute[] { new CategoryAttribute(model.PropertyGroup), new ExpandableObjectAttribute() };
            return new Attribute[] { new CategoryAttribute(model.PropertyGroup) };
        }

        private bool IsExpandable(EditablePropertyModel model)
        {
            if (model.DataType.IsPrimitive)
                return false;
            if (model.DataType == typeof(string))
                return false;
            if (model.DataType.IsEnum)
                return false;
            if (model.DataType == typeof(DateTime))
                return false;
            return true;
        }

        public string GetClassName()
        {
            return GetType().Name;
        }

        private void OnPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (!_dynamicProperties.ContainsKey(e.PropertyName)) return;
            var dynamicProperty = _dynamicProperties[e.PropertyName];
            _data.SetPropertyValue(e.PropertyName, dynamicProperty.Data);
        }

        #endregion

        #region Hide not implemented members
        public AttributeCollection GetAttributes()
        {
            return new AttributeCollection();
            //throw new NotImplementedException();
        }

        public string GetComponentName()
        {
            throw new NotImplementedException();
        }

        public TypeConverter GetConverter()
        {
            return new TypeConverter();
            throw new NotImplementedException();
        }

        public EventDescriptor GetDefaultEvent()
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            throw new NotImplementedException();
        }

        public object GetEditor(Type editorBaseType)
        {
            throw new NotImplementedException();
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        public EventDescriptorCollection GetEvents()
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
            {
                return;
            }

            var eventArgs = new PropertyChangedEventArgs(propertyName);
            PropertyChanged(this, eventArgs);
        }

        private void NotifyToRefreshAllProperties()
        {
            OnPropertyChanged(String.Empty);
        }

        #endregion

        private class DynamicPropertyDescriptor : PropertyDescriptor
        {
            private readonly PropertyEditorModel _editorModel;
            private readonly Type _propertyType;
            private readonly string _propertyName;

            public DynamicPropertyDescriptor(
                PropertyEditorModel editorModel,
                string displayName,
                string propertyName, 
                Type propertyType, 
                Attribute[] propertyAttributes)
                : base(displayName, propertyAttributes)
            {
                _propertyName = propertyName;
                _editorModel = editorModel;
                _propertyType = propertyType;
            }

            public override bool CanResetValue(object component)
            {
                return true;
            }

            public override object GetValue(object component)
            {
                return _editorModel._dynamicProperties[_propertyName].Data;
            }

            public override void ResetValue(object component)
            {
            }

            public override void SetValue(object component, object value)
            {
                var currentValue = _editorModel._dynamicProperties[_propertyName].Data;
                _editorModel._dynamicProperties[_propertyName].Data = value;
                if (currentValue == null && value != null)
                {
                    _editorModel.OnPropertyChanged(_propertyName);
                    return;
                }
                if (currentValue != null && value == null)
                {
                    _editorModel.OnPropertyChanged(_propertyName);
                    return;
                }
                if (currentValue != null && value == null)
                {
                    return;
                }
                if (!currentValue.Equals(value))
                {
                    _editorModel.OnPropertyChanged(_propertyName);
                }
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }

            public override Type ComponentType
            {
                get { return typeof(PropertyEditorModel); }
            }

            public override bool IsReadOnly
            {
                get { return false; }
            }

            public override Type PropertyType
            {
                get { return _propertyType; }
            }
        }
    }
}