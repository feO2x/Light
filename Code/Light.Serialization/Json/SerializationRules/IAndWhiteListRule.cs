using System;
using System.Linq.Expressions;

namespace Light.Serialization.Json.SerializationRules
{
    public interface IAndWhiteListRule<T>
    {
        IAndWhiteListRule<T> AndProperty<TProperty>(Expression<Func<T, TProperty>> propertyExpression);
        IAndWhiteListRule<T> AndField<TField>(Expression<Func<T, TField>> fieldExpression);
    }
}