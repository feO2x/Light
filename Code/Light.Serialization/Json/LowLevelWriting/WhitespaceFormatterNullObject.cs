
namespace Light.Serialization.Json.LowLevelWriting
{
    public sealed class WhitespaceFormatterNullObject : IJsonWhitespaceFormatter
    {
        public int CurrentIndentationLevel { get; private set; }
        public string IdentCharacters { get; set; }

        public void NewlineAndIncreaseIndent(IJsonWriter writer)
        {
            CurrentIndentationLevel++;
        }

        public void Newline(IJsonWriter writer)
        {
        }

        public void NewlineAndDecreaseIndent(IJsonWriter writer)
        {
            CurrentIndentationLevel--;
        }

        public void InsertWhitespaceBetweenKeyAndValue(IJsonWriter writer)
        {
            
        }

        public void ResetIndentationLevel()
        {
            CurrentIndentationLevel = 0;
        }
    }
}