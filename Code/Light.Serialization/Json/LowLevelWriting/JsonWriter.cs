using Light.GuardClauses;
using Light.Serialization.FrameworkExtensions;
using System.IO;

namespace Light.Serialization.Json.LowLevelWriting
{
    public sealed class JsonWriter : IJsonWriter
    {
        private readonly IJsonFormatter _formatter;
        private readonly TextWriter _textWriter;

        public JsonWriter(TextWriter textWriter, IJsonFormatter formatter)
        {
            textWriter.MustNotBeNull(nameof(textWriter));
            formatter.MustNotBeNull(nameof(formatter));

            _textWriter = textWriter;
            _formatter = formatter;
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
            if (shouldNormalizeKey)
                key = NormalizeJsonKey(key);

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

        private static string NormalizeJsonKey(string key)
        {
            return key.FirstCharacterToLowerAndRemoveAllSpecialCharacters();
        }
    }
}