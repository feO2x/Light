using Light.GuardClauses;
using Light.Serialization.FrameworkExtensions;
using System.IO;
using Light.Serialization.Json.LowLevelWriting.KeyNormalization;

namespace Light.Serialization.Json.LowLevelWriting
{
    public sealed class JsonWriter : IJsonWriter
    {
        private readonly IJsonFormatter _formatter;
        private readonly IJsonKeyNormalizer _jsonKeyNormalizer;
        private readonly TextWriter _textWriter;

        public JsonWriter(TextWriter textWriter, IJsonFormatter formatter, IJsonKeyNormalizer jsonKeyNormalizer)
        {
            textWriter.MustNotBeNull(nameof(textWriter));
            formatter.MustNotBeNull(nameof(formatter));
            jsonKeyNormalizer.MustNotBeNull(nameof(jsonKeyNormalizer));

            _textWriter = textWriter;
            _formatter = formatter;
            _jsonKeyNormalizer = jsonKeyNormalizer;
        }

        public void BeginArray()
        {
            _textWriter.Write(JsonSymbols.BeginOfArray);
            _formatter.NewlineAndIncreaseIndent(this);
        }

        public void EndArray()
        {
            _formatter.NewlineAndDecreaseIndent(this);
            _textWriter.Write(JsonSymbols.EndOfArray);
        }

        public void BeginObject()
        {
            _textWriter.Write(JsonSymbols.BeginOfObject);
            _formatter.NewlineAndIncreaseIndent(this);
        }

        public void EndObject()
        {
            _formatter.NewlineAndDecreaseIndent(this);
            _textWriter.Write(JsonSymbols.EndOfObject);
        }

        public void WriteKey(string key, bool shouldNormalizeKey)
        {
            if (_jsonKeyNormalizer.ShouldNormalizeKey)
                key = _jsonKeyNormalizer.Normalize(key);

            if (key.IsSurroundedByQuotationMarks() == false)
                key = key.SurroundWithQuotationMarks();

            _textWriter.Write(key);
            _textWriter.Write(JsonSymbols.PairDelimiter);
            _formatter.InsertWhitespaceBetweenKeyAndValue(this);
        }

        public void WriteDelimiter()
        {
            _textWriter.Write(JsonSymbols.ValueDelimiter);
            _formatter.Newline(this);
        }

        public void WritePrimitiveValue(string @string)
        {
            _textWriter.Write(@string);
        }

        public void WriteNull()
        {
            _textWriter.Write(JsonSymbols.Null);
        }
    }
}