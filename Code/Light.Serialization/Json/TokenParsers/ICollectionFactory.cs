using System;

namespace Light.Serialization.Json.TokenParsers
{
    public interface ICollectionFactory
    {
        object CreateCollection(Type requestedCollectionType);
    }
}