using System;
using Light.GuardClauses;

namespace Light.Serialization.Json.LowLevelWriting
{
    public sealed class IndentingWhitespaceFormatter : IJsonWhitespaceFormatter
    {
        public int CurrentIndentationLevel { get; private set; }

        public string IdentCharacters { get; set; } = "  ";
        private string _whiteSpace = " ";

        public void NewlineAndIncreaseIndent(IJsonWriter writer)
        {
            writer.MustNotBeNull(nameof(writer));

            NewLineWithoutIntent(writer);
            CurrentIndentationLevel++;
            WriteIndent(writer);
        }

        public void Newline(IJsonWriter writer)
        {
            writer.MustNotBeNull(nameof(writer));

            NewLineWithoutIntent(writer);
            WriteIndent(writer);
        }

        public void NewlineAndDecreaseIndent(IJsonWriter writer)
        {
            writer.MustNotBeNull(nameof(writer));

            NewLineWithoutIntent(writer);
            CurrentIndentationLevel--;
            WriteIndent(writer);
        }

        public void InsertWhitespaceBetweenKeyAndValue(IJsonWriter writer)
        {
            writer.MustNotBeNull(nameof(writer));

            writer.WritePrimitiveValue(_whiteSpace);
        }

        public void ResetIndentationLevel()
        {
            CurrentIndentationLevel = 0;
        }

        private void WriteIndent(IJsonWriter writer)
        {
            CurrentIndentationLevel.MustNotBeLessThan(0, nameof(CurrentIndentationLevel));

            for (int i = 0; i < CurrentIndentationLevel; i++)
            {
                writer.WritePrimitiveValue(IdentCharacters);
            }
        }

        private void NewLineWithoutIntent(IJsonWriter writer)
        {
            writer.WritePrimitiveValue(Environment.NewLine);
        }
    }
}