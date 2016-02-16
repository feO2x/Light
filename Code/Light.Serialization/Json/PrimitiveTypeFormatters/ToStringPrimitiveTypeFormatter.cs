namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class ToStringPrimitiveTypeFormatter<T> : BasePrimitiveTypeFormatter<T>, IPrimitiveTypeFormatter
    {
        public ToStringPrimitiveTypeFormatter(bool shouldBeNormalizedKey = true) : base(shouldBeNormalizedKey)
        {

        }

        public string FormatPrimitiveType(object @object)
        {
            return @object.ToString();
        }
    }
}