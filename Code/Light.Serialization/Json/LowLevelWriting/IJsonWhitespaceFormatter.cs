
namespace Light.Serialization.Json.LowLevelWriting
{
    public interface IJsonWhitespaceFormatter
    {
        int CurrentIndentationLevel { get; }
        string IdentCharacters { get; set; }

        void NewlineAndIncreaseIndent(IJsonWriter writer);
        void Newline(IJsonWriter writer);
        void NewlineAndDecreaseIndent(IJsonWriter writer);
        void InsertWhitespaceBetweenKeyAndValue(IJsonWriter writer);
        void ResetIndentationLevel();
    }
}
