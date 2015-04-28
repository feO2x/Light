using System;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public sealed class ValueProviderUsingLambda : IValueProvider
    {
        private readonly string _name;
        private readonly Func<object, object> _lambda;
        private readonly Type _referencedType;

        public ValueProviderUsingLambda(string name, Func<object, object> lambda, Type referencedType)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (lambda == null) throw new ArgumentNullException("lambda");
            if (referencedType == null) throw new ArgumentNullException("referencedType");

            _name = name;
            _lambda = lambda;
            _referencedType = referencedType;
        }

        public string Name { get { return _name; } }

        public Type ReferencedType { get { return _referencedType; } }

        public object GetValue(object @object)
        {
            return _lambda(@object);
        }
    }
}
