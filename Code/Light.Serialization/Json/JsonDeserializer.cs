using System;
using System.Collections.Generic;
using System.IO;

namespace Light.Serialization.Json
{
    public sealed class JsonDeserializer
    {
        private readonly IJsonReaderFactory _jsonReaderFactory;
        private readonly IList<IJsonValueParser> _valueAnalyzers;

        public JsonDeserializer(IJsonReaderFactory jsonReaderFactory, IList<IJsonValueParser> valueAnalyzers)
        {
            if (jsonReaderFactory == null) throw new ArgumentNullException(nameof(jsonReaderFactory));
            if (valueAnalyzers == null) throw new ArgumentNullException(nameof(valueAnalyzers));

            _jsonReaderFactory = jsonReaderFactory;
            _valueAnalyzers = valueAnalyzers;
        }


        public T Deserialize<T>(string json)
        {
            var jsonReader = _jsonReaderFactory.CreateFromString(json);
            return (T) DeserializeValue(jsonReader, typeof(T));
        }

        public object Deserialize(string json, Type requestedType)
        {
            var jsonReader = _jsonReaderFactory.CreateFromString(json);
            return DeserializeValue(jsonReader, requestedType);
        }

        public T Deserialize<T>(Stream jsonStream)
        {
            throw new NotImplementedException();
        }

        private object DeserializeValue(IJsonReader jsonReader, Type requestedType)
        {
            var value = jsonReader.ReadNextValue();
            foreach (var analyzer in _valueAnalyzers)
            {
                if (analyzer.IsSuitableFor(value, requestedType))
                    return analyzer.ParseValue(value, requestedType);
            }


            throw new DeserializationException($"Cannot deserialize value {value} with requested type {requestedType.FullName} because there is no parser that is suitable for this context.");
        }
    }
}