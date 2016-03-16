using System;
using System.Collections.Generic;
using System.Linq;
using Light.GuardClauses;
using Light.Serialization.Json.Caching;
using Light.Serialization.Json.LowLevelReading;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json
{
    public class JsonDeserializerBuilder
    {
        private IJsonReaderFactory _jsonReaderFactory = new SingleBufferJsonReaderFactory();
        private IList<IJsonTokenParser> _jsonTokenParsers;

        private Dictionary<JsonTokenTypeCombination, IJsonTokenParser> _tokenParserCache = new Dictionary<JsonTokenTypeCombination, IJsonTokenParser>();

        public JsonDeserializerBuilder() { }

        public JsonDeserializerBuilder(IList<IJsonTokenParser> jsonTokenParsers)
        {
            jsonTokenParsers.MustNotBeNull(nameof(jsonTokenParsers));
            _jsonTokenParsers = jsonTokenParsers;
        }

        public IList<IJsonTokenParser> JsonTokenParsers => _jsonTokenParsers;

        public JsonDeserializerBuilder WithReaderFactory(IJsonReaderFactory readerFactory)
        {
            _jsonReaderFactory = readerFactory;
            return this;
        }

        public JsonDeserializerBuilder WithTokenParsers(IList<IJsonTokenParser> tokenParsers)
        {
            _jsonTokenParsers = tokenParsers;
            return this;
        }

        public JsonDeserializerBuilder WithTokenParserCache(Dictionary<JsonTokenTypeCombination, IJsonTokenParser> cache)
        {
            _tokenParserCache = cache;
            return this;
        }

        public JsonDeserializerBuilder ConfigureTokenParser<T>(Action<T> configureParser)
            where T : IJsonTokenParser
        {
            configureParser(_jsonTokenParsers.OfType<T>().First());
            return this;
        }

        public JsonDeserializerBuilder AddTokenParserBefore<T>(IJsonTokenParser additionalParser)
            where T : IJsonTokenParser
        {
            additionalParser.MustNotBeNull(nameof(additionalParser));

            var targetIndex = _jsonTokenParsers.IndexOf(_jsonTokenParsers.OfType<T>().First());
            if (targetIndex == -1)
                throw new ArgumentException($"The specified IJsonTokenParser {additionalParser} could not be added before the {typeof (T)} parser because the latter one could not be found.");

            _jsonTokenParsers.Insert(targetIndex, additionalParser);
            return this;
        }

        public JsonDeserializerBuilder AddTokenParserAfter<T>(IJsonTokenParser additionalParser)
            where T : IJsonTokenParser
        {
            additionalParser.MustNotBeNull(nameof(additionalParser));

            var targetTindex = _jsonTokenParsers.IndexOf(_jsonTokenParsers.OfType<T>().First());
            if (targetTindex == -1)
                throw new ArgumentException($"The specified IJsonTokenParser {additionalParser} could not be added after the {typeof (T)} parser because the latter one could not be found.");

            if (targetTindex == _jsonTokenParsers.Count - 1)
                _jsonTokenParsers.Add(additionalParser);
            else
                _jsonTokenParsers.Insert(targetTindex + 1, additionalParser);

            return this;
        }

        public JsonDeserializer Build()
        {
            var jsonTokenParsers = _jsonTokenParsers ?? new DefaultTokenParsersBuilder().Build();

            return new JsonDeserializer(_jsonReaderFactory, (IReadOnlyList<IJsonTokenParser>) jsonTokenParsers, _tokenParserCache);
        }
    }
}