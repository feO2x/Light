using System;

namespace Light.Serialization.Json.JsonValueParsers
{
    public class DefaultGenericCollectionFactory : ICollectionFactory
    {
        public object CreateCollection(Type requestedCollectionType)
        {
            if (requestedCollectionType.IsClass &&
                requestedCollectionType.IsAbstract == false)
            {
                var defaultConstructor = requestedCollectionType.GetConstructor(null);
                if (defaultConstructor != null)
                    return defaultConstructor.Invoke(null);
            }
            throw new NotImplementedException();
        }
    }
}