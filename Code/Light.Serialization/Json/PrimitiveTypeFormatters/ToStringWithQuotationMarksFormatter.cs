using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class ToStringWithQuotationMarksFormatter<T> : BasePrimitiveTypeFormatter<T>, IPrimitiveTypeFormatter
    {
        public ToStringWithQuotationMarksFormatter(bool shouldBeNormalizedKey = true) : base(shouldBeNormalizedKey)
        {
        }

        public string FormatPrimitiveType(object @object)
        {
            return @object.ToString().SurroundWithQuotationMarks();
        }
    }
}
