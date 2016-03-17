using Light.GuardClauses.Exceptions;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Light.GuardClauses
{
    public static class StringGuardClauses
    {
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeNullOrEmpty(this string @string, string parameterName)
        {
            if (@string == null)
                throw new ArgumentNullException(parameterName);

            if (@string == string.Empty)
                throw new EmptyStringException(parameterName);
        }

        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustMatch(this string parameter, Regex pattern, string parameterName)
        {
            var match = pattern.Match(parameter);
            if (match.Success == false)
                throw new StringDoesNotMatchException(parameterName, parameter, pattern);
        }

        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeEmpty(this Guid parameter, string parameterName)
        {
            if (parameter == Guid.Empty)
                throw new EmptyGuidException(parameterName);
        }


        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeNullOrWhiteSpace(this string parameter, string parameterName)
        {
            parameter.MustNotBeNullOrEmpty(parameterName);

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var character in parameter)
            {
                if (char.IsWhiteSpace(character) == false)
                    return;
            }
            throw new StringIsOnlyWhiteSpaceException(parameterName, parameter);
        }

        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustContain(this string parameter, string containedText, string parameterName)
        {
            if (parameter.Contains(containedText) == false)
                throw new StringException($"{parameterName} must contain the text \"{containedText}\", but you specified \"{parameter}\".", parameterName);
        }

        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotContain(this string parameter, string textToCompare, string parameterName)
        {
            if (parameter.Contains(textToCompare))
                throw new StringException($"{parameterName} must not contain the text \"{textToCompare}\", but you specified \"{parameter}\".", parameterName);
        }
    }
}