﻿using System;
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
    
    public class ActorPropertyEditorModel : DynamicObject, ICustomTypeDescriptor, INotifyPropertyChanged
    {
        private readonly IDictionary<string, ActorEditablePropertyModel> dynamicProperties =
            new Dictionary<string, ActorEditablePropertyModel>();
        private BaseActor _ba;

        public ActorPropertyEditorModel(BaseActor ba)
        {
            _ba = ba;
            var editableProps =
                ba.GetType()
                .GetProperties()
                .Where(prop => prop.ContainsAttribute(typeof(EditableAttribute))) 
                .Where(prop => prop.CanBeGettedAndSetted())
                .Select(prop => ConvertToActorEditableProperty(prop))
                .ToList();

            foreach (var editableProp in editableProps)
                AddProperty(editableProp);
        }

        private ActorEditablePropertyModel ConvertToActorEditableProperty(PropertyInfo prop)
        {
            var editable = 
                (EditableAttribute)(Attribute.GetCustomAttributes(prop, typeof(EditableAttribute), true).First());

            var propGroup = editable.EditorGroup;
            var propName = editable.EditorName;

            if (String.IsNullOrWhiteSpace(propGroup))
                propGroup = ActorPropertyEditorDefaultValues.DefaultGroupName;

            if (String.IsNullOrWhiteSpace(propName))
                propName = prop.Name;
            
            return new ActorEditablePropertyModel()
            {
                PropertyGroup = propGroup,
                PropertyName = propName,
                Data = prop.GetValue(_ba, null),
                DataType = prop.PropertyType
            };
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var memberName = binder.Name;
            ActorEditablePropertyModel bucket;
            var gotValue = dynamicProperties.TryGetValue(memberName, out bucket);
            result = bucket.Data;
            return gotValue;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var memberName = binder.Name;
            var model = new ActorEditablePropertyModel() 
            {
                Data = value,
                DataType = binder.ReturnType,
                PropertyName = memberName,
                PropertyGroup = ActorPropertyEditorDefaultValues.DefaultGroupName
            };
            AddProperty(model);
            return true;
        }

        protected void AddProperty(ActorEditablePropertyModel editableProp)
        {
            dynamicProperties[editableProp.PropertyName] = editableProp;
            NotifyToRefreshAllProperties();
        }


        #region Implementation of ICustomTypeDescriptor

        public PropertyDescriptorCollection GetProperties()
        {
            // of course, here must be the attributes associated
            // with each of the dynamic properties

            var properties = dynamicProperties
                .Select(pair => new DynamicPropertyDescriptor(this,
                    pair.Key, pair.Value.DataType, GetPropertyEditorAttributesFrom(pair.Value)));
            return new PropertyDescriptorCollection(properties.ToArray());
        }

        private Attribute[] GetPropertyEditorAttributesFrom(ActorEditablePropertyModel model)
        {
            if (IsExpandable(model))
                return new Attribute[] { new CategoryAttribute(model.PropertyGroup), new ExpandableObjectAttribute() };
            return new Attribute[] { new CategoryAttribute(model.PropertyGroup) };
        }

        private bool IsExpandable(ActorEditablePropertyModel model)
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
            private readonly ActorPropertyEditorModel obj;
            private readonly Type propertyType;

            public DynamicPropertyDescriptor(ActorPropertyEditorModel obj,
                string propertyName, Type propertyType, Attribute[] propertyAttributes)
                : base(propertyName, propertyAttributes)
            {
                this.obj = obj;
                this.propertyType = propertyType;
            }

            public override bool CanResetValue(object component)
            {
                return true;
            }

            public override object GetValue(object component)
            {
                return obj.dynamicProperties[Name].Data;
            }

            public override void ResetValue(object component)
            {
            }

            public override void SetValue(object component, object value)
            {
                obj.dynamicProperties[Name].Data = value;
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }

            public override Type ComponentType
            {
                get { return typeof(ActorPropertyEditorModel); }
            }

            public override bool IsReadOnly
            {
                get { return false; }
            }

            public override Type PropertyType
            {
                get { return propertyType; }
            }
        }
    }
}