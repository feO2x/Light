using System;
using System.IO;
using System.Text;

namespace Light.Serialization.Json
{
    public sealed class JsonWriterFactory : IJsonWriterFactory
    {
        private StringBuilder _stringBuilder;
        private IJsonFormatter _jsonFormatter = new JsonFormatterNullObject();
        private StringWriter _stringWriter;

        public IJsonFormatter JsonFormatter
        {
            get { return _jsonFormatter; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _jsonFormatter = value;
            }
        }

        public IJsonWriter Create()
        {
            _stringBuilder = new StringBuilder();
            _stringWriter = new StringWriter(_stringBuilder);
            return new JsonWriter(_stringWriter, _jsonFormatter);
        }

        public string FinishWriteProcessAndReleaseResources()
        {
            var returnValue = _stringBuilder.ToString();
            _stringWriter = null;
            _stringBuilder = null;
            return returnValue;
        }
    }
}