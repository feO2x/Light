using System.Text;
using Light.GuardClauses;

namespace Light.Serialization.FrameworkExtensions
{
    public static class StringBuilderExtensions
    {
        public static string CompleteJsonStringWithQuotationMark(this StringBuilder stringBuilder)
        {
            stringBuilder.MustNotBeNull(nameof(stringBuilder));

            stringBuilder.Append('"');
            return stringBuilder.ToString();
        }
    }
}