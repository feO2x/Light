using Light.GuardClauses;
using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class CharFormatter : BasePrimitiveTypeFormatter<char>, IPrimitiveTypeFormatter
    {
        private ICharacterEscaper _characterEscaper;

        public ICharacterEscaper CharacterEscaper
        {
            get { return _characterEscaper; }
            set
            {
                value.MustNotBeNull(nameof(value));
                _characterEscaper = value;
            }
        }

        public CharFormatter(ICharacterEscaper characterEscaper) : base(true)
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