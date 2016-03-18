using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    /// <summary>
    ///     The StringAssertions class contains extension methods that make assertions on <see cref="string" /> instances.
    /// </summary>
    public static class StringAssertions
    {
        /// <summary>
        ///     Ensures that the specified <paramref name="parameter" /> is not null or empty, or otherwise throws an exception.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="ArgumentNullException" /> or
        ///     <see cref="EmptyStringException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is either null or empty (optional). Please
        ///     note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify
        ///     exception.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null and no
        ///     <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="EmptyStringException">
        ///     Thrown when <paramref name="parameter" /> is empty and no
        ///     <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeNullOrEmpty(this string parameter, string parameterName = null, string message = null, Exception exception = null)
        {
            if (parameter == null)
                throw exception ?? new ArgumentNullException(parameterName, message);

            if (parameter == string.Empty)
                throw exception ?? (message == null ? new EmptyStringException(parameterName) : new EmptyStringException(message, parameterName));
        }

        /// <summary>
        ///     Ensures that <paramref name="parameter" /> matches the specified regular expression, or otherwise throws an
        ///     <see cref="StringDoesNotMatchException" />.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="pattern">The regular expression used to evaluate <paramref name="parameter" />.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="StringDoesNotMatchException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does not match the
        ///     <paramref name="pattern" /> (optional). Please note that <paramref name="message" /> and
        ///     <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringDoesNotMatchException">
        ///     Thrown when <paramref name="parameter" /> does not match the
        ///     <paramref name="pattern" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustMatch(this string parameter, Regex pattern, string parameterName = null, string message = null, Exception exception = null)
        {
            var match = pattern.Match(parameter);
            if (match.Success == false)
                throw exception ?? (message == null ? new StringDoesNotMatchException(parameterName, parameter, pattern) : new StringDoesNotMatchException(message, parameterName));
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