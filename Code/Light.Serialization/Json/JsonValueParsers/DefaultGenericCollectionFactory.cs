using System;

namespace Light.Serialization.Json.JsonValueParsers
{
    public class DefaultGenericCollectionFactory : ICollectionFactory
    {
        private readonly Type[] _emptyTypeArray = new Type[0];

        public object CreateCollection(Type requestedCollectionType)
        {
            if (requestedCollectionType.IsClass &&
                requestedCollectionType.IsAbstract == false)
            {
                var defaultConstructor = requestedCollectionType.GetConstructor(_emptyTypeArray);
                if (defaultConstructor != null)
                    return defaultConstructor.Invoke(null);
            }
            throw new NotImplementedException();
        }
    }
}