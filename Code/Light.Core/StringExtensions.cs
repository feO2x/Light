using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Core
{
    public static class StringExtensions
    {
        public static string SurroundWith(this string @string, string value)
        {
            if (value == string.Empty)
                return @string;
            return @string.Insert(@string.Length, value).Insert(0, value);
        }

        public static string SurroundWith(this string @string, char character)
        {
            var length = @string.Length;
            return @string.PadRight(++length, character).PadLeft(++length, character);
        }

        public static string SurroundWithQuotationMarks(this string @string)
        {
            return @string.SurroundWith('"');
        }

        public static string SurroundWithParantheses(this string @string)
        {
            var length = @string.Length;
            return @string.PadLeft(++length, '(').PadRight(++length, ')');
        }

        public static bool IsSurroundedByQuotationMarks(this string @string)
        {
            if (@string.Length <= 1)
                return false;
            return @string[0] == '"' && @string[@string.Length - 1] == '"';
        }
    }
}
