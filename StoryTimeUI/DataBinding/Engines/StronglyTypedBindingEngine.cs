using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace StoryTimeUI.DataBinding.Engines
{
    public class BindingEngine<TDestination, TSource> : BindingEngine
    {
        private readonly TDestination _destination;
        private readonly TSource _source;

        public BindingEngine(TDestination destination, TSource source)
            :base(destination, source)
        {
            _destination = destination;
            _source = source;
        }

        public BindingEngine<TDestination, TSource> Bind<TData>(
            Expression<Func<TDestination, TData>> destinationExpression,
            Expression<Func<TSource, TData>> sourceExpression,
            BindingType bindingType = BindingType.OneWayToDestination)
        {
            MemberExpression destinationMember = GetMemberExpressionFrom(destinationExpression.Body);
            MemberExpression sourceMember = GetMemberExpressionFrom(sourceExpression.Body);

            PropertyInfo destinationPropInfo = destinationMember.Member as PropertyInfo;
            PropertyInfo sourcePropInfo = sourceMember.Member as PropertyInfo;

            AssignBindersWith(destinationPropInfo, sourcePropInfo, bindingType);

            return this;
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
