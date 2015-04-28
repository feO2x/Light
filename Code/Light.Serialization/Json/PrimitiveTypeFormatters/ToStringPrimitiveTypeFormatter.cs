
namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class ToStringPrimitiveTypeFormatter<T> : BasePrimitiveTypeFormatter<T>, IPrimitiveTypeFormatter
    {
        public string FormatPrimitiveType(object @object)
        {
            return @object.ToString();
        }
    }
}
