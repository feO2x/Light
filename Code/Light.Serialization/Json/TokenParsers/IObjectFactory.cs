using System.Collections.Generic;

namespace Light.Serialization.Json.TokenParsers
{
    public interface IObjectFactory
    {
        object Create(TypeConstructionInfo typeConstructionInfo, Dictionary<InjectableValueInfo, object> deserializedChildValues);
    }
}