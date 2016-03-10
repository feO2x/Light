using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.Caching;
using Light.Serialization.Json.ComplexTypeConstruction;
using Light.Serialization.Json.LowLevelReading;
using Light.Serialization.Json.TokenParsers;
using Light.Serialization.Json.TypeNaming;

namespace Light.Serialization.Json
{
    public class JsonDeserializerBuilder
    {
        private ICollectionFactory _collectionFactory = new DefaultGenericCollectionFactory();
        private IJsonReaderFactory _jsonReaderFactory = new SingleBufferJsonReaderFactory();

        private IReadOnlyList<IJsonTokenParser> _jsonTokenParsers;
        private IInjectableValueNameNormalizer _nameNormalizer = new ToLowerWithoutSpecialCharactersNormalizer();
        private INameToTypeMapping _nameToTypeMapping = new SimpleNameToTypeMapping();
        private IObjectFactory _objectFactory = new DefaultObjectFactory();
        private ITypeDescriptionProvider _typeDescriptionProvider;
        private JsonTokenParserCache _jsonTokenParserCache = new JsonTokenParserCache(new List<JsonTokenTypeCombination>());

        public JsonDeserializerBuilder()
        {
            _typeDescriptionProvider = new TypeDescriptionCacheDecorator(new DefaultTypeDescriptionProvider(_nameNormalizer), new Dictionary<Type, TypeCreationDescription>());
        }

        public JsonDeserializerBuilder WithReaderFactory(IJsonReaderFactory readerFactory)
        {
            _jsonReaderFactory = readerFactory;
            return this;
        }

        public JsonDeserializerBuilder WithTokenParsers(IReadOnlyList<IJsonTokenParser> tokenParsers)
        {
            _jsonTokenParsers = tokenParsers;
            return this;
        }

        public JsonDeserializerBuilder WithObjectFactory(IObjectFactory objectFactory)
        {
            _objectFactory = objectFactory;
            return this;
        }

        public JsonDeserializerBuilder WithNameNormalizer(IInjectableValueNameNormalizer nameNormalizer)
        {
            _nameNormalizer = nameNormalizer;
            return this;
        }

        public JsonDeserializerBuilder WithNameToTypeMapping(INameToTypeMapping mapping)
        {
            _nameToTypeMapping = mapping;
            return this;
        }

        public JsonDeserializerBuilder WithTypeCreationInfoAnalyzer(ITypeDescriptionProvider analyzer)
        {
            _typeDescriptionProvider = analyzer;
            return this;
        }

        public JsonDeserializerBuilder WithCollectionFactory(ICollectionFactory collectionFactory)
        {
            _collectionFactory = collectionFactory;
            return this;
        }

        public JsonDeserializerBuilder WithJsonTokenParserCache(JsonTokenParserCache jsonTokenParserCache)
        {
            jsonTokenParserCache.MustNotBeNull(nameof(jsonTokenParserCache));
            _jsonTokenParserCache = jsonTokenParserCache;
            return this;
        }

        public JsonDeserializer Build()
        {
            if (_jsonTokenParsers == null)
            {
                _jsonTokenParsers = new List<IJsonTokenParser>().AddDefaultTokenParsers(_collectionFactory,
                                                                                        _objectFactory,
                                                                                        _nameToTypeMapping,
                                                                                        _nameNormalizer,
                                                                                        _typeDescriptionProvider);
            }

            return new JsonDeserializer(_jsonReaderFactory, _jsonTokenParsers, _jsonTokenParserCache);
        }
    }
}