using System.Collections.Generic;
using Light.Serialization.Json.JsonValueDeserilizers;

namespace Light.Serialization.Json
{
    public class JsonDeserializerBuilder
    {
        private readonly IJsonReaderFactory _jsonReaderFactory = new JsonReaderFactory();

        private readonly IList<IJsonValueDeserializer> _jsonValueDeserializers = new IJsonValueDeserializer[]
                                                                                 {
                                                                                     new IntDeserializer()
                                                                                 };

        public JsonDeserializer Build()
        {
            return new JsonDeserializer(_jsonReaderFactory, _jsonValueDeserializers);
        }
    }
}