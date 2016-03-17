using System;
using System.Diagnostics;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses
{
    /// <summary>
    ///     This class contains the most common assertions like MustNotBeNull and assertions that are not directly related to
    ///     categories like collection assertions or string assertions.
    /// </summary>
    public static class CommonAssertions
    {
        /// <summary>
        ///     Ensures that the specified parameter is not <c>null</c>, and otherwise throws an
        ///     <see cref="ArgumentNullException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that should be injected into the <see cref="ArgumentNullException" /> (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when the specified parameter is <c>null</c>.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null) where T : class
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName, message);
        }

        /// <summary>
        ///     Ensures that the specified parameter is not <c>null</c>, and otherwise throws the specified exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="otherwiseCreateException">The delegate that creates the exception to be thrown.</param>
        public static void MustNotBeNull<T>(this T parameter, Func<Exception> otherwiseCreateException) where T : class
        {
            if (parameter == null)
                throw otherwiseCreateException();
        }

        /// <summary>
        ///     Ensures that the specified parameter is <c>null</c>, and otherwise throws an
        ///     <see cref="ArgumentNotNullException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that should be injected in to the <see cref="ArgumentNotNullException" /> (optional). Please note that parameterName is ignored when you use message.</param>
        /// <exception cref="ArgumentNotNullException">Thrown when the specified parameter is not <c>null</c>.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeNull<T>(this T parameter, string parameterName = null, string message = null) where T : class
        {
            if (parameter != null)
                throw message == null ? new ArgumentNotNullException(parameterName, parameter) : new ArgumentNotNullException(message);
        }

        /// <summary>
        ///     Ensures that the specified parameter is <c>null</c>, and otherwise throws the specified exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="otherwiseCreateException">The delegate that creates the exception to be thrown.</param>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeNull<T>(this T parameter, Func<Exception> otherwiseCreateException)
        {
            if (parameter != null)
                throw otherwiseCreateException();
        }

        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBe<T>(this T parameter, T other, string parameterName)
        {
            if (parameter.EqualsWithHashCode(other) == false)
                throw new ArgumentException($"{parameterName} must be {other}, but you specified {parameter}.", parameterName);
        }

        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBe<T>(this T parameter, T other, Exception exception)
        {
            if (parameter.EqualsWithHashCode(other) == false)
                throw exception;
        }

        public static TOut MustBeType<TOut>(this object @object, string parameterName) where TOut : class
        {
            var castedValue = @object as TOut;
            if (castedValue == null)
                throw new TypeMismatchException(parameterName, $"{parameterName} is of type {@object.GetType().FullName} and cannot be downcasted to {typeof (TOut).FullName}.");

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