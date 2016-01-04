using System.Collections.Generic;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json
{
    public class JsonDeserializerBuilder
    {
        private readonly IJsonReaderFactory _jsonReaderFactory = new JsonReaderFactory();

        private readonly IList<IJsonTokenParser> _jsonValueDeserializers = new IJsonTokenParser[]
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