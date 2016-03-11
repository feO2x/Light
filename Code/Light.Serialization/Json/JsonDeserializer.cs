using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.Caching;

namespace Light.Serialization.Json
{
    public sealed class JsonDeserializer : IDeserializer
    {
        private readonly Dictionary<JsonTokenTypeCombination, IJsonTokenParser> _cache;
        private readonly IJsonReaderFactory _jsonReaderFactory;
        private readonly IReadOnlyList<IJsonTokenParser> _tokenParsers;
        private IJsonReader _jsonReader;

        public JsonDeserializer(IJsonReaderFactory jsonReaderFactory,
                                IReadOnlyList<IJsonTokenParser> tokenParsers,
                                Dictionary<JsonTokenTypeCombination, IJsonTokenParser> cache)
        {
            jsonReaderFactory.MustNotBeNull(nameof(jsonReaderFactory));
            tokenParsers.MustNotBeNull(nameof(tokenParsers));
            cache.MustNotBeNull(nameof(cache));

            _jsonReaderFactory = jsonReaderFactory;
            _tokenParsers = tokenParsers;
            _cache = cache;
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
            IJsonTokenParser parser;

            var tokenTypeCombination = new JsonTokenTypeCombination(token.JsonType, requestedType);
            if (_cache.TryGetValue(tokenTypeCombination, out parser) == false)
            {
                foreach (var tokenParser in _tokenParsers)
                {
                    if (tokenParser.IsSuitableFor(token, requestedType) == false)
                        continue;

                    parser = tokenParser;
                    break;
                }

                if (parser == null)
                    throw new DeserializationException($"Cannot deserialize value {token} with requested type {requestedType.FullName} because there is no parser that is suitable for this context.");

                if (parser.CanBeCached)
                    _cache.Add(tokenTypeCombination, parser);
            }

            return parser.ParseValue(new JsonDeserializationContext(token, requestedType, _jsonReader, DeserializeJsonToken));
        }
    }
}