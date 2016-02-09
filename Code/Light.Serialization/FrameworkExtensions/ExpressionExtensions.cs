using System;
using System.Linq.Expressions;
using System.Reflection;
using Light.GuardClauses;

namespace Light.Serialization.FrameworkExtensions
{
    public static class ExpressionExtensions
    {
        private const string InvalidPropertyExceptionMessage = "The specified expression is not valid. Please use an expression like the following one: o => o.Property";
        private const string InvalidFieldExceptionMessage = "The specified fieldSelector is not valid. Please use an expression like the following one: o => o.Field";

        public static string ExtractPropertyName<T, TProperty>(this Expression<Func<T, TProperty>> expression)
        {
            expression.MustNotBeNull(nameof(expression));

            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException(InvalidPropertyExceptionMessage, nameof(expression));
            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new ArgumentException(InvalidPropertyExceptionMessage, nameof(expression));

            if (propertyInfo.CanRead == false)
                throw new ArgumentException($"The specified property {propertyInfo.Name} has no get method and thus cannot be used for serialization.", "expression");

            return propertyInfo.Name;
        }

        public static string ExtractFieldName<T, TField>(this Expression<Func<T, TField>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException(InvalidFieldExceptionMessage, nameof(expression));

            var fieldInfo = memberExpression.Member as FieldInfo;
            if (fieldInfo == null)
                throw new ArgumentException(InvalidFieldExceptionMessage, nameof(expression));

            return fieldInfo.Name;
        }
    }
}
