namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class BooleanFormatter : BasePrimitiveTypeFormatter<bool>, IPrimitiveTypeFormatter
    {
        public string FormatPrimitiveType(object @object)
        {
            return @object.ToString().ToLower();
        }
    }
}