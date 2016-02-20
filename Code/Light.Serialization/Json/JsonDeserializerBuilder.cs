using System;
using System.Collections.Generic;
using Light.Serialization.Json.Caching;
using Light.Serialization.Json.ComplexTypeConstruction;
using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.LowLevelReading;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json
{
    public class JsonDeserializerBuilder
    {
        public readonly JsonReaderSymbols JsonReaderSymbols = new JsonReaderSymbols();

        private ICollectionFactory _collectionFactory = new DefaultGenericCollectionFactory();
        private IJsonReaderFactory _jsonReaderFactory = new SingleBufferJsonReaderFactory();

        private IList<IJsonTokenParser> _jsonTokenParsers;
        private IInjectableValueNameNormalizer _nameNormalizer = new ToLowerWithoutSpecialCharactersNormalizer();
        private INameToTypeMapping _nameToTypeMapping = new SimpleNameToTypeMapping();
        private IObjectFactory _objectFactory = new DefaultObjectFactory();
        private ITypeDescriptionProvider _typeDescriptionProvider;

        public JsonDeserializerBuilder()
        {
            _typeDescriptionProvider = new TypeDescriptionCacheDecorator(new DefaultTypeDescriptionProvider(_nameNormalizer), new Dictionary<Type, TypeCreationDescription>());
        }

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

        public JsonDeserializerBuilder ConfigureJsonReaderSymbols(Action<JsonReaderSymbols> configureSymbols)
        {
            configureSymbols(JsonReaderSymbols);
            return this;
        }

        public JsonDeserializerBuilder WithCollectionFactory(ICollectionFactory collectionFactory)
        {
            _collectionFactory = collectionFactory;
            return this;
        }

        public JsonDeserializer Build()
        {
            if (_jsonTokenParsers == null)
            {
                _jsonTokenParsers = new List<IJsonTokenParser>().AddDefaultTokenParsers(JsonReaderSymbols,
                                                                                        _collectionFactory,
                                                                                        _objectFactory,
                                                                                        _nameToTypeMapping,
                                                                                        _nameNormalizer,
                                                                                        _typeDescriptionProvider);
            }

            return new JsonDeserializer(_jsonReaderFactory, _jsonTokenParsers);
        }
    }
}