using System;

namespace Light.Serialization
{
    public interface ISerializer
    {
        string Serialize<T>(T objectGraphRoot);
        string Serialize(object objectGraphRoot, Type referencedType);
    }
}
