﻿using System.Text;
using Light.GuardClauses;
using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class StringFormatter : BasePrimitiveTypeFormatter<string>, IPrimitiveTypeFormatter
    {
        private ICharacterEscaper _characterEscaper;

        public ICharacterEscaper CharacterEscaper
        {
            get { return _characterEscaper;}
            set
            {
                value.MustNotBeNull(nameof(value));
                _characterEscaper = value;
            }
        }

        public StringFormatter(ICharacterEscaper characterEscaper) : base(true)
        {
            characterEscaper.MustNotBeNull(nameof(characterEscaper));

            _characterEscaper = characterEscaper;
        }

        public string FormatPrimitiveType(object @object)
        {
            var @string = (string) @object;

            char[] characterBuffer;
            var i = 0;
            for (; i < @string.Length; i++)
            {
                characterBuffer = _characterEscaper.Escape(@string[i]);
                if (characterBuffer != null)
                    goto EscapeStringContent;
            }
            return @string.SurroundWithQuotationMarks();

            EscapeStringContent:
            var stringBuilder = new StringBuilder();
            stringBuilder.Append('"');
            stringBuilder.Append(@string.Substring(0, i));
            stringBuilder.Append(characterBuffer);
            i++;
            for (; i < @string.Length; i++)
            {
                var character = @string[i];
                characterBuffer = _characterEscaper.Escape(character);
                if (characterBuffer == null)
                {
                    stringBuilder.Append(character);
                    continue;
                }

                stringBuilder.Append(characterBuffer);
            }
            stringBuilder.Append('"');
            return stringBuilder.ToString();
        }
    }
}