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
        public BindingEngine(TDestination destination, TSource source)
            :base(destination, source)
        {
        }

        public BindingEngine<TDestination, TSource> Bind<TData>(
            Expression<Func<TDestination, TData>> destinationExpression,
            Expression<Func<TSource, TData>> sourceExpression,
            BindingType bindingType = BindingType.OneWayToDestination)
        {
            var destinationMember = GetMemberExpressionFrom(destinationExpression.Body);
            var sourceMember = GetMemberExpressionFrom(sourceExpression.Body);

            var destinationPropInfo = destinationMember.Member as PropertyInfo;
            var sourcePropInfo = sourceMember.Member as PropertyInfo;

            AssignBindersWith(destinationPropInfo, sourcePropInfo, bindingType);

            return this;
        }

        private MemberExpression GetMemberExpressionFrom(Expression expressionBody)
        {
            var body = expressionBody as MemberExpression;

            if (body == null)
            {
                body = ((UnaryExpression)expressionBody).Operand as MemberExpression;
            }
            return body;
        }
    }
}
