using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.TokenParsers
{
    public interface IObjectFactory
    {
        object Create(Type requestedType, Dictionary<InjectableValueInfo, object> deserializedChildValues);
    }
}