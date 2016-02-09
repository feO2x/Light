using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public sealed class ValueProviderFactoryUsingLambdas : IValueProviderFactory
    {
        private static readonly Type TypeOfObject = typeof (object);

        public IValueProvider Create(Type targetType, PropertyInfo propertyInfo)
        {
            var parameterExpression = Expression.Parameter(TypeOfObject);

            Expression bodyExpression = Expression.Property(Expression.ConvertChecked(parameterExpression, targetType), propertyInfo);

            bodyExpression = CheckForPossiblyNeededValueTypeConversion(bodyExpression, propertyInfo.PropertyType.GetTypeInfo());

            var lambda = Expression.Lambda<Func<object, object>>(bodyExpression, parameterExpression).Compile();

            return new ValueProviderUsingLambda(propertyInfo.Name, lambda, propertyInfo.PropertyType);
        }

        public IValueProvider Create(Type targetType, FieldInfo fieldInfo)
        {
            var parameterExpression = Expression.Parameter(TypeOfObject);

            Expression bodyExpression = Expression.Field(Expression.ConvertChecked(parameterExpression, targetType), fieldInfo);

            bodyExpression = CheckForPossiblyNeededValueTypeConversion(bodyExpression, fieldInfo.FieldType.GetTypeInfo());

            var lambda = Expression.Lambda<Func<object, object>>(bodyExpression, parameterExpression).Compile();

            return new ValueProviderUsingLambda(fieldInfo.Name, lambda, fieldInfo.FieldType);
        }

        private static Expression CheckForPossiblyNeededValueTypeConversion(Expression bodyExpression, TypeInfo returnedSubType)
        {
            return returnedSubType.IsValueType ? Expression.Convert(bodyExpression, TypeOfObject) : bodyExpression;
        }
    }
}