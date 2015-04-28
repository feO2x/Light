using Light.Core;

namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class ToStringWithQuotationMarksFormatter<T> : BasePrimitiveTypeFormatter<T>, IPrimitiveTypeFormatter
    {
        public string FormatPrimitiveType(object @object)
        {
            return @object.ToString().SurroundWithQuotationMarks();
        }
    }
}
