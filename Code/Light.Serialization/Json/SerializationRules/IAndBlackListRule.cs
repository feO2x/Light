using System;
using System.Linq.Expressions;

namespace Light.Serialization.Json.SerializationRules
{
    public interface IAndBlackListRule<T>
    {
        IAndBlackListRule<T> IgnoreProperty<TProperty>(Expression<Func<T, TProperty>> propertyExpression);
        IAndBlackListRule<T> IgnoreField<TField>(Expression<Func<T, TField>> fieldExpression);
    }
}
