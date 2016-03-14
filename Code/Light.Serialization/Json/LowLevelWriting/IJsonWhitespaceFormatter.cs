
namespace Light.Serialization.Json.LowLevelWriting
{
    public interface IJsonWhitespaceFormatter
    {
        int CurrentIndentationLevel { get; }

        void NewlineAndIncreaseIndent(IJsonWriter writer);
        void Newline(IJsonWriter writer);
        void NewlineAndDecreaseIndent(IJsonWriter writer);
        void InsertWhitespaceBetweenKeyAndValue(IJsonWriter writer);
        void ResetIndentationLevel();
    }
}
