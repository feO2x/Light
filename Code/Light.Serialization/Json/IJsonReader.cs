namespace Light.Serialization.Json
{
    public interface IJsonReader
    {
        JsonCharacterBuffer ReadNextValue();
    }
}