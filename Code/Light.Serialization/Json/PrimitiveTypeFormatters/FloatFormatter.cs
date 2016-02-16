using System.Globalization;
using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class FloatFormatter : BasePrimitiveTypeFormatter<float>, IPrimitiveTypeFormatter
    {
        public FloatFormatter() : base(false)
        {
            
        }

        public string FormatPrimitiveType(object @object)
        {
            var value = (float) @object;

            return FormatRoundTripFloatValue(value, value.ToString("R", CultureInfo.InvariantCulture));
        }

        private static string FormatRoundTripFloatValue(float value, string text)
        {
            if (float.IsInfinity(value) || float.IsNaN(value))
                return text.SurroundWithQuotationMarks();

            if (text.IndexOf('.') == -1 && text.IndexOf('E') == -1 && text.IndexOf('e') == -1)
                return text + ".0";

            return text;
        }
    }
}