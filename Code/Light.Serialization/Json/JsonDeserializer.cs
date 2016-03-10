using Light.GuardClauses;
using System;
using System.Collections.Generic;
using Light.Serialization.Json.Caching;

namespace Light.Serialization.Json
{
    public sealed class JsonDeserializer : IDeserializer
    {
        private readonly IJsonReaderFactory _jsonReaderFactory;
        private readonly IReadOnlyList<IJsonTokenParser> _tokenParsers;
        private readonly JsonTokenParserCache _jsonTokenParserCache;
        private IJsonReader _jsonReader;

        public JsonDeserializer(IJsonReaderFactory jsonReaderFactory, IReadOnlyList<IJsonTokenParser> tokenParsers, JsonTokenParserCache jsonTokenParserCache)
        {
            jsonReaderFactory.MustNotBeNull(nameof(jsonReaderFactory));
            tokenParsers.MustNotBeNull(nameof(tokenParsers));
            jsonTokenParserCache.MustNotBeNull(nameof(jsonTokenParserCache));

            _jsonReaderFactory = jsonReaderFactory;
            _tokenParsers = tokenParsers;
            _jsonTokenParserCache = jsonTokenParserCache;
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
            IJsonTokenParser parser = null;

            if (_jsonTokenParserCache.CheckJsonTokenTypeForBlacklist(token, requestedType) == false)
            {
                _jsonTokenParserCache.TryGetJsonTokenParser(token, requestedType, out parser);
            }
            
            if(parser == null)
            { 
                for(int i = 0; i < _tokenParsers.Count; i++)
                {
                    var tokenParser = _tokenParsers[i];
                    if (tokenParser.IsSuitableFor(token, requestedType))
                    { 
                        parser = tokenParser;
                        i = _tokenParsers.Count;
                    }
                }
            }

            if(parser == null)
                throw new DeserializationException($"Cannot deserialize value {token} with requested type {requestedType.FullName} because there is no parser that is suitable for this context.");

            return parser.ParseValue(new JsonDeserializationContext(token, requestedType, _jsonReader, DeserializeJsonToken));
        }
    }
}