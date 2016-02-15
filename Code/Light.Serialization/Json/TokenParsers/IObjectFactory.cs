using System.Collections.Generic;

namespace Light.Serialization.Json.TokenParsers
{
    public interface IObjectFactory
    {
        object Create(TypeCreationInfo typeCreationInfo, Dictionary<InjectableValueInfo, object> deserializedChildValues);
    }
}