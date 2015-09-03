using System;
using System.Collections.Generic;
using System.IO;

namespace Light.Serialization.Json
{
    public sealed class JsonDeserializer
    {
        private readonly IJsonReaderFactory _jsonReaderFactory;
        private readonly IList<IJsonValueDeserializer> _valueAnalyzers;

        public JsonDeserializer(IJsonReaderFactory jsonReaderFactory, IList<IJsonValueDeserializer> valueAnalyzers)
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
                    return analyzer.DeserializeValue(value, requestedType);
            }


            throw new DeserializationException($"Cannot deserialize value {value} with requested type {requestedType.FullName} because there is no value deserilizer that is suitable for this context.");
        }
    }
}