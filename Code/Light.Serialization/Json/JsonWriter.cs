using System;
using System.IO;
using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json
{
    public sealed class JsonWriter : IJsonWriter
    {
        private readonly IJsonFormatter _formatter;
        private readonly TextWriter _textWriter;
        private readonly JsonWriterTokens _tokens;

        public JsonWriter(TextWriter textWriter, IJsonFormatter formatter, JsonWriterTokens tokens)
        {
            if (textWriter == null) throw new ArgumentNullException(nameof(textWriter));
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            if (tokens == null) throw new ArgumentNullException(nameof(tokens));

            _textWriter = textWriter;
            _formatter = formatter;
            _tokens = tokens;
        }

        public void BeginArray()
        {
            _textWriter.Write(_tokens.BeginCollectionToken);
            _formatter.NewlineAndIncreaseIndent(this);
        }

        public void EndArray()
        {
            _formatter.NewlineAndDecreaseIndent(this);
            _textWriter.Write(_tokens.EndCollectionToken);
        }

        public void BeginObject()
        {
            _textWriter.Write(_tokens.BeginComplexObjectToken);
            _formatter.NewlineAndIncreaseIndent(this);
        }

        public void EndObject()
        {
            _formatter.NewlineAndDecreaseIndent(this);
            _textWriter.Write(_tokens.EndComplexObjectToken);
        }

        public void WriteKey(string key)
        {
            if (key.IsSurroundedByQuotationMarks() == false)
                key = key.SurroundWithQuotationMarks();

            _textWriter.Write(key);
            _textWriter.Write(_tokens.KeyValueDelimiter);
            _formatter.InsertWhitespaceBetweenKeyAndValue(this);
        }

        public void WriteDelimiter()
        {
            _textWriter.Write(_tokens.ValueDelimiter);
            _formatter.Newline(this);
        }

        public void WritePrimitiveValue(string @string)
        {
            _textWriter.Write(@string);
        }

        public void WriteNull()
        {
            _textWriter.Write(_tokens.Null);
        }
    }
}