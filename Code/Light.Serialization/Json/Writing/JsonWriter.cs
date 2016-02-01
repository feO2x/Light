using System;
using System.IO;
using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.Writing
{
    public sealed class JsonWriter : IJsonWriter
    {
        private readonly IJsonFormatter _formatter;
        private readonly TextWriter _textWriter;
        private readonly JsonWriterSymbols _symbols;

        public JsonWriter(TextWriter textWriter, IJsonFormatter formatter, JsonWriterSymbols symbols)
        {
            if (textWriter == null) throw new ArgumentNullException(nameof(textWriter));
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            if (symbols == null) throw new ArgumentNullException(nameof(symbols));

            _textWriter = textWriter;
            _formatter = formatter;
            _symbols = symbols;
        }

        public void BeginArray()
        {
            _textWriter.Write(_symbols.BeginOfArray);
            _formatter.NewlineAndIncreaseIndent(this);
        }

        public void EndArray()
        {
            _formatter.NewlineAndDecreaseIndent(this);
            _textWriter.Write(_symbols.EndOfArray);
        }

        public void BeginObject()
        {
            _textWriter.Write(_symbols.BeginOfObject);
            _formatter.NewlineAndIncreaseIndent(this);
        }

        public void EndObject()
        {
            _formatter.NewlineAndDecreaseIndent(this);
            _textWriter.Write(_symbols.EndOfObject);
        }

        public void WriteKey(string key)
        {
            if (key.IsSurroundedByQuotationMarks() == false)
                key = key.SurroundWithQuotationMarks();

            _textWriter.Write(key);
            _textWriter.Write(_symbols.PairDelimiter);
            _formatter.InsertWhitespaceBetweenKeyAndValue(this);
        }

        public void WriteDelimiter()
        {
            _textWriter.Write(_symbols.ValueDelimiter);
            _formatter.Newline(this);
        }

        public void WritePrimitiveValue(string @string)
        {
            _textWriter.Write(@string);
        }

        public void WriteNull()
        {
            _textWriter.Write(_symbols.Null);
        }
    }
}