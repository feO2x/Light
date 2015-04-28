using Light.Core;
using System;
using System.IO;

namespace Light.Serialization.Json
{
    public sealed class JsonWriter : IJsonWriter
    {
        private readonly TextWriter _streamWriter;
        private readonly IJsonFormatter _formatter;

        public JsonWriter(TextWriter writer)
            : this(writer, new JsonFormatterNullObject())
        {

        }

        public JsonWriter(TextWriter streamWriter, IJsonFormatter formatter)
        {
            if (streamWriter == null) throw new ArgumentNullException("streamWriter");
            if (formatter == null) throw new ArgumentNullException("formatter");

            _streamWriter = streamWriter;
            _formatter = formatter;
        }

        public void BeginCollection()
        {
            _streamWriter.Write('[');
            _formatter.NewlineAndIncreaseIndent(this);
        }

        public void EndCollection()
        {
            _formatter.NewlineAndDecreaseIndent(this);
            _streamWriter.Write(']');
        }

        public void BeginComplexObject()
        {
            _streamWriter.Write('{');
            _formatter.NewlineAndIncreaseIndent(this);
        }

        public void EndComplexObject()
        {
            _formatter.NewlineAndDecreaseIndent(this);
            _streamWriter.Write('}');

        }

        public void WriteKey(string key)
        {
            if (key.IsSurroundedByQuotationMarks() == false)
                key = key.SurroundWithQuotationMarks();

            _streamWriter.Write(key);
            _streamWriter.Write(':');
            _formatter.InsertWhitespaceBetweenKeyAndValue(this);
        }

        public void WriteDelimiter()
        {
            _streamWriter.Write(',');
            _formatter.Newline(this);
        }

        public void WriteRaw(string @string)
        {
            _streamWriter.Write(@string);
        }

        public void WriteNull()
        {
            _streamWriter.Write("null");
        }
    }
}
