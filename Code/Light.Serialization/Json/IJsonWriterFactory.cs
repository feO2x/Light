using Light.Serialization.Json.LowLevelWriting;

namespace Light.Serialization.Json
{
    public interface IJsonWriterFactory
    {
        IJsonWriter Create();
        string FinishWriteProcessAndReleaseResources();
        IJsonWhitespaceFormatter JsonWhitespaceFormatter { get; set; }
    }
}