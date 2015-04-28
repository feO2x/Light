﻿using Light.Core;
using System;
using System.Globalization;

namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class DoubleFormatter : IPrimitiveTypeFormatter
    {
        private readonly Type _targetType = typeof (double);

        public Type TargetType { get { return _targetType; } }

        public string FormatPrimitiveType(object @object)
        {
            var value = (double) @object;

            return FormatRoundtripDoubleValue(value, value.ToString("R", CultureInfo.InvariantCulture));
        }

        private static string FormatRoundtripDoubleValue(double value, string text)
        {
            if (double.IsInfinity(value) || double.IsNaN(value))
                return text.SurroundWithQuotationMarks();

            if (text.IndexOf('.') == -1 && text.IndexOf('E') == -1 && text.IndexOf('e') == -1)
                return text + ".0";

            return text;
        }
    }
}
