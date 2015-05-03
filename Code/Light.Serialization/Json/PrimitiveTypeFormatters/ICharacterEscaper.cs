namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public interface ICharacterEscaper
    {
        char[] Escape(char character);
    }
}