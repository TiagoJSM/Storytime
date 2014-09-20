using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace StoryTimeUI.DataBinding
{
    public class BindingEngine<TDestination, TSource> where TSource : INotifyPropertyChanged
    {
        private readonly TDestination _destination;
        private readonly TSource _source;
        //first PropertyInfo is for destination, second is for source
        private readonly Dictionary<string, Tuple<PropertyInfo, PropertyInfo>> _propertyMapping;

        public BindingEngine(TDestination destination, TSource source)
        {
            _destination = destination;
            _source = source;

            //This is the source property to destination property mapping
            _propertyMapping = new Dictionary<string, Tuple<PropertyInfo, PropertyInfo>>();

            //listen for INotifyPropertyChanged event on the source
            source.PropertyChanged += SourcePropertyChanged;
        }

        public BindingEngine<TDestination, TSource> Bind<TData>(
            Expression<Func<TDestination, TData>> destinationExpression,
            Expression<Func<TSource, TData>> sourceExpression)
        {
            MemberExpression destinationMember = GetMemberExpressionFrom(destinationExpression.Body);
            MemberExpression sourceMember = GetMemberExpressionFrom(sourceExpression.Body);

            PropertyInfo destinationPropInfo = destinationMember.Member as PropertyInfo;
            PropertyInfo sourcePropInfo = sourceMember.Member as PropertyInfo;

            _propertyMapping.Add(
                sourcePropInfo.Name, 
                new Tuple<PropertyInfo, PropertyInfo>(destinationPropInfo, sourcePropInfo));

            return this;
        }

        private void SourcePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (_propertyMapping.ContainsKey(args.PropertyName))
            {
                Tuple<PropertyInfo, PropertyInfo> tuple = _propertyMapping[args.PropertyName];
                tuple.Item1.SetValue(_destination, tuple.Item2.GetValue(_source, null), null);
            }
        }

        private MemberExpression GetMemberExpressionFrom(Expression expressionBody)
        {
            MemberExpression body = expressionBody as MemberExpression;

            if (body == null)
            {
                body = ((UnaryExpression)expressionBody).Operand as MemberExpression;
            }
            return body;
        }
    }
}
