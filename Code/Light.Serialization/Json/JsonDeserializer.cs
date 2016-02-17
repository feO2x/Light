using Light.GuardClauses;
using System;
using System.Collections.Generic;

namespace Light.Serialization.Json
{
    public sealed class JsonDeserializer : IDeserializer
    {
        private readonly IJsonReaderFactory _jsonReaderFactory;
        private readonly IList<IJsonTokenParser> _tokenParsers;
        private IJsonReader _jsonReader;

        public JsonDeserializer(IJsonReaderFactory jsonReaderFactory, IList<IJsonTokenParser> tokenParsers)
        {
            jsonReaderFactory.MustNotBeNull(nameof(jsonReaderFactory));
            tokenParsers.MustNotBeNull(nameof(tokenParsers));

            _jsonReaderFactory = jsonReaderFactory;
            _tokenParsers = tokenParsers;
        }


        public T Deserialize<T>(string json)
        {
            var result = Deserialize(json, typeof (T));
            return (T) result;
        }

        public object Deserialize(string json, Type requestedType)
        {
            _jsonReader = _jsonReaderFactory.CreateFromString(json);
            var returnValue = DeserializeDocument(requestedType);
            _jsonReader = null;
            return returnValue;
        }

        private object DeserializeDocument(Type requestedType)
        {
            var token = _jsonReader.ReadNextToken();
            return DeserializeJsonToken(token, requestedType);
        }

        private object DeserializeJsonToken(JsonToken token, Type requestedType)
        {
            foreach (var parser in _tokenParsers)
            {
                if (parser.IsSuitableFor(token, requestedType))
                    return parser.ParseValue(new JsonDeserializationContext(token, requestedType, _jsonReader, DeserializeJsonToken));
            }

            throw new DeserializationException($"Cannot deserialize value {token} with requested type {requestedType.FullName} because there is no parser that is suitable for this context.");
        }
    }
}