using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Light.GuardClauses;

namespace Light.Serialization.Json.LowLevelWriting
{
    public sealed class JsonWriterFactory : IJsonWriterFactory
    {
        private readonly List<Func<IJsonWriter, IJsonWriter>> _decorateFunctions = new List<Func<IJsonWriter, IJsonWriter>>();
        private IJsonFormatter _jsonFormatter = new JsonFormatterNullObject();
        private JsonWriterSymbols _jsonWriterSymbols = new JsonWriterSymbols();
        private StringBuilder _stringBuilder;
        private StringWriter _stringWriter;

        public IJsonFormatter JsonFormatter
        {
            get { return _jsonFormatter; }
            set
            {
                value.MustNotBeNull(nameof(value));
                _jsonFormatter = value;
            }
        }

        public JsonWriterSymbols JsonWriterSymbols
        {
            get { return _jsonWriterSymbols; }
            set
            {
                value.MustNotBeNull(nameof(value));
                _jsonWriterSymbols = value;
            }
        }

        public IJsonWriter Create()
        {
            _stringBuilder = new StringBuilder();
            _stringWriter = new StringWriter(_stringBuilder);
            IJsonWriter returnValue = new JsonWriter(_stringWriter, _jsonFormatter, _jsonWriterSymbols);
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var decorateFunction in _decorateFunctions)
            {
                returnValue = decorateFunction(returnValue);
            }
            return returnValue;
        }

        public string FinishWriteProcessAndReleaseResources()
        {
            var returnValue = _stringBuilder.ToString();
            _stringWriter = null;
            _stringBuilder = null;
            return returnValue;
        }

        public JsonWriterFactory DecorateCreationWith(Func<IJsonWriter, IJsonWriter> decoratorFunction)
        {
            decoratorFunction.MustNotBeNull(nameof(decoratorFunction));

            _decorateFunctions.Add(decoratorFunction);
            return this;
        }
    }
}