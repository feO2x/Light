using System;
using System.Collections.Generic;
using Light.Serialization.Json.LowLevelReading;
using Light.Serialization.Json.ObjectConstruction;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json
{
    public class JsonDeserializerBuilder
    {
        public readonly JsonReaderSymbols JsonReaderSymbols = new JsonReaderSymbols();

        private ICollectionFactory _collectionFactory = new DefaultGenericCollectionFactory();
        private IJsonReaderFactory _jsonReaderFactory = new SingleBufferJsonReaderFactory();
        private IList<IJsonTokenParser> _jsonTokenParsers;

        public JsonDeserializerBuilder()
        {
            _jsonTokenParsers = new List<IJsonTokenParser>().AddDefaultTokenParsers(JsonReaderSymbols, _collectionFactory);
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
            return new JsonDeserializer(_jsonReaderFactory, _jsonTokenParsers);
        }
    }
}