using System.Collections.Generic;
using Light.Serialization.Json.JsonValueParsers;

namespace Light.Serialization.Json
{
    public class JsonDeserializerBuilder
    {
        private readonly IJsonReaderFactory _jsonReaderFactory = new JsonReaderFactory();

        private readonly IList<IJsonValueParser> _jsonValueDeserializers = new IJsonValueParser[]
                                                                                 {
                                                                                     new IntParser()
                                                                                 };

        public JsonDeserializer Build()
        {
            return new JsonDeserializer(_jsonReaderFactory, _jsonValueDeserializers);
        }
    }
}