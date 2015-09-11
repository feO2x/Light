using System;

namespace Light.Serialization.Json.JsonValueParsers
{
    public interface ICollectionFactory
    {
        object CreateCollection(Type requestedCollectionType);
    }
}