using System;
using System.Linq.Expressions;

namespace Dapperism.Extensions.Extensions
{
    public static class LambdaExt
    {
        public static string GetMemberName<TSource, TProperty>(this Expression<Func<TSource, TProperty>> property)
        {
            if (Equals(property, null))
            {
                throw new NullReferenceException("Property is required");
            }

            MemberExpression expr;

            if (property.Body is MemberExpression)
            {
                expr = (MemberExpression)property.Body;
            }
            else if (property.Body is UnaryExpression)
            {
                expr = (MemberExpression)((UnaryExpression)property.Body).Operand;
            }
            else
            {
                const string format = "Expression '{0}' not supported.";
                var message = string.Format(format, property);

                throw new ArgumentException(message, "Property");
            }

            return expr.Member.Name;
        }
    }
}
