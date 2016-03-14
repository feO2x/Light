using System;

namespace Light.Serialization.Json.LowLevelWriting
{
    public sealed class IndentingWhitespaceFormatter : IJsonWhitespaceFormatter
    {
        private string _indentCharacters = "  "; // Encapsulate

        public int CurrentIndentationLevel { get; }

        public void NewlineAndIncreaseIndent(IJsonWriter writer)
        {
            writer.WritePrimitiveValue(Environment.NewLine);
            writer.WritePrimitiveValue(_indentCharacters);
        }

        public void Newline(IJsonWriter writer)
        {
            throw new NotImplementedException();
        }

        public void NewlineAndDecreaseIndent(IJsonWriter writer)
        {
            throw new NotImplementedException();
        }

        public void InsertWhitespaceBetweenKeyAndValue(IJsonWriter writer)
        {
            throw new NotImplementedException();
        }

        public void ResetIndentationLevel()
        {
            throw new NotImplementedException();
        }
    }
}