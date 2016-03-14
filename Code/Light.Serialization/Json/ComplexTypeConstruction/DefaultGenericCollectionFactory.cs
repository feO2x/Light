using System;
using System.Linq;
using System.Reflection;
using Light.GuardClauses;

namespace Light.Serialization.Json.ComplexTypeConstruction
{
    public sealed class DefaultGenericCollectionFactory : ICollectionFactory
    {
        public object CreateCollection(Type requestedCollectionType)
        {
            requestedCollectionType.MustNotBeNull(nameof(requestedCollectionType));

            var typeInfo = requestedCollectionType.GetTypeInfo();
            if (typeInfo.IsClass &&
                typeInfo.IsAbstract == false)
            {
                var defaultConstructor = typeInfo.DeclaredConstructors.FirstOrDefault(c => c.GetParameters().Length == 0);
                if (defaultConstructor != null)
                    return defaultConstructor.Invoke(null);

                throw new ArgumentException($"Could not instantiate collection type {requestedCollectionType} because this type has no default constructor.");
            }
            throw new NotImplementedException("What happens with collection that do not have a default constructor?");
        }
    }
}