using System;
using System.Diagnostics;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    /// <summary>
    ///     This class contains the most common assertions like MustNotBeNull and assertions that are not directly related to
    ///     categories like collection assertions or string assertions.
    /// </summary>
    public static class CommonAssertions
    {
        /// <summary>
        ///     Ensures that the specified parameter is not <c>null</c>, or otherwise throws an
        ///     <see cref="ArgumentNullException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that should be injected into the <see cref="ArgumentNullException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is <c>null</c> (optional). Please
        ///     note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify
        ///     exception.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the specified parameter is <c>null</c> and no
        ///     <paramref name="exception" /> was specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null, Exception exception = null) where T : class
        {
            if (parameter == null)
                throw exception ?? new ArgumentNullException(parameterName, message);
        }

        /// <summary>
        ///     Ensures that the specified parameter is <c>null</c>, or otherwise throws an <see cref="ArgumentNotNullException" />
        ///     .
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="ArgumentNotNullException" /> (optional).
        ///     Please note that <paramref name="parameterName" /> is ignored when you use message.
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is not <c>null</c> (optional). Please
        ///     note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify
        ///     exception.
        /// </param>
        /// <exception cref="ArgumentNotNullException">
        ///     Thrown when the specified parameter is not <c>null</c> and no
        ///     <paramref name="exception" /> was specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeNull<T>(this T parameter, string parameterName = null, string message = null, Exception exception = null) where T : class
        {
            if (parameter != null)
                throw exception ?? (message == null ? new ArgumentNotNullException(parameterName, parameter) : new ArgumentNotNullException(message));
        }

        /// <summary>
        ///     Ensures that parameter is of the specified type and returns the downcasted value, or throws a
        ///     <see cref="TypeMismatchException" /> otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeMismatchException" /> (optional).
        ///     Please note that <paramref name="parameterName" /> is ignored when you use message.
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> cannot be downcasted (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you
        ///     specify exception.
        /// </param>
        /// <returns>The downcasted reference to <paramref name="parameter" />.</returns>
        public static T MustBeType<T>(this object parameter, string parameterName = null, string message = null, Exception exception = null) where T : class
        {
            var castedValue = parameter as T;
            if (castedValue == null)
                throw exception ?? new TypeMismatchException(parameterName, message ?? $"{parameterName ?? "The object"} is of type {parameter.GetType().FullName} and cannot be downcasted to {typeof (T).FullName}.");

            return castedValue;
        }

        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustHaveValue<T>(this T? parameter, string parameterName) where T : struct
        {
            if (parameter.HasValue == false)
                throw new NullableHasNoValueException(parameterName);
        }

        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotHaveValue<T>(this T? parameter, string parameterName) where T : struct
        {
            if (parameter.HasValue)
                throw new NullableHasValueException(parameterName, parameter.Value);
        }

        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeValidEnumValue<T>(this T parameter, string parameterName)
        {
            var enumType = typeof (T);
            if (Enum.IsDefined(enumType, parameter) == false)
                throw new EnumValueNotDefinedException(parameterName, parameter, enumType);
        }
    }
}