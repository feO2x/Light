using System;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public sealed class ValueProviderUsingLambda : IValueProvider
    {
        private readonly Func<object, object> _lambda;

        public ValueProviderUsingLambda(string name, Func<object, object> lambda, Type referenceType)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (lambda == null) throw new ArgumentNullException(nameof(lambda));
            if (referenceType == null) throw new ArgumentNullException(nameof(referenceType));

            Name = name;
            _lambda = lambda;
            ReferenceType = referenceType;
        }

        public string Name { get; }

        public Type ReferenceType { get; }

        public object GetValue(object @object)
        {
            return _lambda(@object);
        }

        public override string ToString()
        {
            return $"ValueProviderUsingLambda for {ReferenceType.FullName}.{Name}";
        }
    }
}