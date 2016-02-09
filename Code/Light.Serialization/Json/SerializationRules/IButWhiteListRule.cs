using System;
using System.Linq.Expressions;

namespace Light.Serialization.Json.SerializationRules
{
    public interface IButWhiteListRule<T>
    {
        IAndWhiteListRule<T> ButProperty<TProperty>(Expression<Func<T, TProperty>> propertyExpression);
        IAndWhiteListRule<T> ButField<TField>(Expression<Func<T, TField>> fieldExpression);
    }
}