using System.Collections.Generic;

namespace Light.Serialization.Json.ComplexTypeConstruction
{
    public interface IObjectFactory
    {
        object Create(TypeCreationDescription typeCreationDescription, Dictionary<InjectableValueDescription, object> deserializedChildValues);
    }
}