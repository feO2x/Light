using Light.Serialization.Json.TokenParsers;
using System;
using System.Linq;
using System.Reflection;

namespace Light.Serialization.Json.ObjectConstruction
{
    public class DefaultGenericCollectionFactory : ICollectionFactory
    {
        private readonly Type[] _emptyTypeArray = new Type[0];

        public object CreateCollection(Type requestedCollectionType)
        {
            var typeInfo = requestedCollectionType.GetTypeInfo();

            if (typeInfo.IsClass &&
                typeInfo.IsAbstract == false)
            {
                var defaultConstructor = typeInfo.DeclaredConstructors.First(c => c.GetParameters().Length == 0);
                return defaultConstructor.Invoke(null); // TODO: I have to throw a proper exception here if the call to First fails
            }
            throw new NotImplementedException();
        }
    }
}