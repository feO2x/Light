using System;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public sealed class ValueProviderUsingLambda : IValueProvider
    {
        private readonly string _name;
        private readonly Func<object, object> _lambda;
        private readonly Type _referenceType;

        public ValueProviderUsingLambda(string name, Func<object, object> lambda, Type referenceType)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (lambda == null) throw new ArgumentNullException("lambda");
            if (referenceType == null) throw new ArgumentNullException("referenceType");

            _name = name;
            _lambda = lambda;
            _referenceType = referenceType;
        }

        public string Name { get { return _name; } }

        public Type ReferenceType { get { return _referenceType; } }

        public object GetValue(object @object)
        {
            return _lambda(@object);
        }
    }
}
