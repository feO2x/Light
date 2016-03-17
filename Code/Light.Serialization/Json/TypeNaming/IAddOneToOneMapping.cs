using System;

namespace Light.Serialization.Json.TypeNaming
{
    public interface IAddOneToOneMapping
    {
        void AddMapping(string jsonName, Type correspondingType);
    }
}