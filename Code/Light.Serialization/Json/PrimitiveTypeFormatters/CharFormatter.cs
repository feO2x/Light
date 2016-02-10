using Light.GuardClauses;
using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class CharFormatter : BasePrimitiveTypeFormatter<char>, IPrimitiveTypeFormatter
    {
        private readonly ICharacterEscaper _characterEscaper;

        public CharFormatter(ICharacterEscaper characterEscaper)
        {
            characterEscaper.MustNotBeNull(nameof(characterEscaper));

            _characterEscaper = characterEscaper;
        }

        public string FormatPrimitiveType(object @object)
        {
            var value = (char) @object;

            var characterBuffer = _characterEscaper.Escape(value);
            var jsonRepresenation = characterBuffer != null ? new string(characterBuffer) : value.ToString();

            return jsonRepresenation.SurroundWithQuotationMarks();
        }
    }
}