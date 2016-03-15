using System;
using Light.GuardClauses;

namespace Light.Serialization.Json.LowLevelWriting
{
    public sealed class IndentingWhitespaceFormatter : IJsonWhitespaceFormatter
    {
        private readonly string _whiteSpace = " ";
        private int _currentIndentationLevel;
        private string _indentCharacters = "  ";

        public int CurrentIndentationLevel => _currentIndentationLevel;

        public string IdentCharacters
        {
            get { return _indentCharacters; }
            set
            {
                value.MustNotBeNull(nameof(value));
                _indentCharacters = value;
            }
        }

        public void NewlineAndIncreaseIndent(IJsonWriter writer)
        {
            writer.MustNotBeNull(nameof(writer));

            NewLineWithoutIntent(writer);
            _currentIndentationLevel++;
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
            _currentIndentationLevel--;
            WriteIndent(writer);
        }

        public void InsertWhitespaceBetweenKeyAndValue(IJsonWriter writer)
        {
            writer.MustNotBeNull(nameof(writer));

            writer.WritePrimitiveValue(_whiteSpace);
        }

        public void ResetIndentationLevel()
        {
            _currentIndentationLevel = 0;
        }

        private void WriteIndent(IJsonWriter writer)
        {
            for (var i = 0; i < _currentIndentationLevel; i++)
            {
                writer.WritePrimitiveValue(IdentCharacters);
            }
        }

        private static void NewLineWithoutIntent(IJsonWriter writer)
        {
            writer.WritePrimitiveValue(Environment.NewLine);
        }
    }
}