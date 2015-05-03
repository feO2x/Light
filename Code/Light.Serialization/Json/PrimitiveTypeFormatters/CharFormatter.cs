﻿using Light.Core;
using System;

namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class CharFormatter : IPrimitiveTypeFormatter
    {
        private readonly ICharacterEscaper _characterEscaper;
        private readonly Type _targetType = typeof (char);

        public Type TargetType
        {
            get { return _targetType; }
        }

        public CharFormatter(ICharacterEscaper characterEscaper)
        {
            if (characterEscaper == null) throw new ArgumentNullException("characterEscaper");

            _characterEscaper = characterEscaper;
        }

        public string FormatPrimitiveType(object @object)
        {
            var value = (char) @object;

            string jsonRepresenation;
            if (_characterEscaper.Escape(value, out jsonRepresenation) == false)
                jsonRepresenation = @object.ToString();

            return jsonRepresenation.SurroundWithQuotationMarks();
        }
    }
}