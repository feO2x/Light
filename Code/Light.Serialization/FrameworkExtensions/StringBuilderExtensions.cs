using System.Text;

namespace Light.Serialization.FrameworkExtensions
{
    public static class StringBuilderExtensions
    {
        public static string CompleteJsonStringWithQuotationMarkAndClearBuilder(this StringBuilder stringBuilder)
        {
            stringBuilder.Append('"');

            var result = stringBuilder.ToString();
            stringBuilder.Clear();
            return result;
        }
    }
}