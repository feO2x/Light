
namespace Light.Serialization.Json.Writing
{
    public sealed class JsonFormatterNullObject : IJsonFormatter
    {
        public int CurrentIndentationLevel { get; private set; }

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