using Light.Serialization.Json.JsonValueParsers;
using System.Collections.Generic;

namespace Light.Serialization.Json
{
    public class JsonDeserializerBuilder
    {
        private readonly IJsonReaderFactory _jsonReaderFactory = new JsonReaderFactory();

        private readonly IList<IJsonValueParser> _jsonValueDeserializers = new IJsonValueParser[]
                                                                           {
                                                                               new IntParser(),
                                                                               new StringParser(),
                                                                               new DoubleParser(),
                                                                               new NullParser(),
                                                                               new CharacterParser(),
                                                                               new BooleanParser(),
                                                                               new ArrayToGenericCollectionParser(new DefaultGenericCollectionFactory())
                                                                           };

        public JsonDeserializer Build()
        {
            return new JsonDeserializer(_jsonReaderFactory, _jsonValueDeserializers);
        }
    }
}