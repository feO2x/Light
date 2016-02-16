using System.Globalization;

namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class DecimalFormatter : BasePrimitiveTypeFormatter<decimal>, IPrimitiveTypeFormatter
    {
        public DecimalFormatter() : base(false)
        {
            
        }
        
        public string FormatPrimitiveType(object @object)
        {
            var value = (decimal)@object;

            return FormatDecimal(value.ToString(CultureInfo.InvariantCulture));
        }

        private static string FormatDecimal(string text)
        {
            if (text.IndexOf('.') == -1)
                return text + ".0";
            return text;
        }
    }
}
