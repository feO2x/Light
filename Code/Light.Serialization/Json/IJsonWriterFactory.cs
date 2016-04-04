using Light.Serialization.Json.LowLevelWriting;

namespace Light.Serialization.Json
{
    public interface IJsonWriterFactory
    {
        IJsonWriter Create();
        string FinishWriteProcessAndReleaseResources();
    }
}