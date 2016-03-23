using System;
using System.Linq;
using System.Reflection;
using Light.GuardClauses;

namespace Light.Serialization.Json.ComplexTypeConstruction
{
    public sealed class DefaultGenericDictionaryFactory : IDictionaryFactory
    {
        public object CreateDictionary(Type requestedDictionaryType)
        {
            requestedDictionaryType.MustNotBeNull(nameof(requestedDictionaryType));

            var typeInfo = requestedDictionaryType.GetTypeInfo();
            if (typeInfo.IsClass &&
                typeInfo.IsAbstract == false)
            {
                var defaultConstructor = typeInfo.DeclaredConstructors.First(c => c.GetParameters().Length == 0);
                if (defaultConstructor != null)
                    return defaultConstructor.Invoke(null);
            }

            throw new NotImplementedException("What happens with collection that do not have a default constructor?");
        }
    }
}