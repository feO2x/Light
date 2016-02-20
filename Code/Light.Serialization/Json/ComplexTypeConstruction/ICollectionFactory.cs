using System;

namespace Light.Serialization.Json.ComplexTypeConstruction
{
    public interface ICollectionFactory
    {
        object CreateCollection(Type requestedCollectionType);
    }
}