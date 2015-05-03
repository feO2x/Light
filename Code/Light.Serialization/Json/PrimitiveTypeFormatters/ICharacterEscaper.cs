namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public interface ICharacterEscaper
    {
        bool Escape(char character, out string result);
    }
}