using System;
using System.IO;
using System.Text;

namespace Light.Serialization.Json
{
    public sealed class JsonDocumentWriter : IDocumentWriter, IJsonWriter
    {
        private readonly IJsonFormatter _formatter;
        private string _document;
        private int _initialCapacity = 1024;
        private StringBuilder _stringBuilder;
        private JsonWriter _internalWriter;
        private StringWriter _stringWriter;

        public JsonDocumentWriter()
            : this(new JsonFormatterNullObject()) { }

        public JsonDocumentWriter(IJsonFormatter formatter)
        {
            if (formatter == null) throw new ArgumentNullException("formatter");

            _formatter = formatter;
        }

        public string Document
        {
            get
            {
                if (_document != null) return _document;

                return _stringBuilder == null ? null : _stringBuilder.ToString();
            }
        }

        public int InitialCapacity
        {
            get { return _initialCapacity; }
            set
            {
                if (value < 2) throw new ArgumentOutOfRangeException("value", "InitialCapacity cannot be less than 2");
                _initialCapacity = value;
            }
        }

        public void BeginDocument()
        {
            _document = null;
            _stringBuilder = new StringBuilder();
            _stringWriter = new StringWriter(_stringBuilder);
            _internalWriter = new JsonWriter(_stringWriter, _formatter);
        }

        public void EndDocument()
        {
            _stringWriter.Flush();
            _document = _stringBuilder.ToString();
            _internalWriter = null;
            _stringWriter = null;
            _stringBuilder = null;
        }

        public void BeginCollection()
        {
            _internalWriter.BeginCollection();
        }

        public void WriteDelimiter()
        {
            _internalWriter.WriteDelimiter();
        }

        public void EndCollection()
        {
            _internalWriter.EndCollection();
        }

        public void BeginComplexObject()
        {
            _internalWriter.BeginComplexObject();
        }

        public void EndComplexObject()
        {
            _internalWriter.EndComplexObject();
        }

        public void WriteKey(string key)
        {
            _internalWriter.WriteKey(key);
        }

        public void WriteRaw(string @string)
        {
            _internalWriter.WriteRaw(@string);
        }

        public void WriteNull()
        {
            _internalWriter.WriteNull();
        }
    }
}
