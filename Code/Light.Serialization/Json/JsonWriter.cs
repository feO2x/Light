using System;
using System.IO;
using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json
{
    public sealed class JsonWriter : IJsonWriter
    {
        private readonly TextWriter _textWriter;
        private readonly IJsonFormatter _formatter;

        public JsonWriter(TextWriter textWriter)
            : this(textWriter, new JsonFormatterNullObject())
        {

        }

        public JsonWriter(TextWriter textWriter, IJsonFormatter formatter)
        {
            if (textWriter == null) throw new ArgumentNullException(nameof(textWriter));
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));

            _textWriter = textWriter;
            _formatter = formatter;
        }

        public void BeginCollection()
        {
            _textWriter.Write('[');
            _formatter.NewlineAndIncreaseIndent(this);
        }

        public void EndCollection()
        {
            _formatter.NewlineAndDecreaseIndent(this);
            _textWriter.Write(']');
        }

        public void BeginComplexObject()
        {
            _textWriter.Write('{');
            _formatter.NewlineAndIncreaseIndent(this);
        }

        public void EndComplexObject()
        {
            _formatter.NewlineAndDecreaseIndent(this);
            _textWriter.Write('}');

        }

        public void WriteKey(string key)
        {
            if (key.IsSurroundedByQuotationMarks() == false)
                key = key.SurroundWithQuotationMarks();

            _textWriter.Write(key);
            _textWriter.Write(':');
            _formatter.InsertWhitespaceBetweenKeyAndValue(this);
        }

        public void WriteDelimiter()
        {
            _textWriter.Write(',');
            _formatter.Newline(this);
        }

        public void WriteRaw(string @string)
        {
            _textWriter.Write(@string);
        }

        public void WriteNull()
        {
            _textWriter.Write("null");
        }
    }
}
