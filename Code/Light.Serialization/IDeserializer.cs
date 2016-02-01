using System;

namespace Light.Serialization
{
    public interface IDeserializer
    {
        T Deserialize<T>(string json);

        object Deserialize(string json, Type requestedType);
    }
}
