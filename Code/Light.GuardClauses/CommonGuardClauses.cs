using System;
using System.Diagnostics;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses
{
    public static class CommonGuardClauses
    {
        [Conditional(Check.PreconditionSymbol)]
        public static void MustNotBeNull(this object parameter, string parameterName)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName);
        }

        [Conditional(Check.PreconditionSymbol)]
        public static void MustBeNull(this object parameter, string parameterName)
        {
            if (parameter != null)
                throw new ArgumentNotNullException(parameterName, parameter);
        }

        [Conditional(Check.PreconditionSymbol)]
        public static void MustBe<T>(this T parameter, T other, string parameterName)
        {
            if (parameter.EqualsWithHashCode(other) == false)
                throw new ArgumentException($"{parameterName} must be {other}, but you specified {parameter}.", parameterName);
        }

        [Conditional(Check.PreconditionSymbol)]
        public static void MustBe<T>(this T parameter, T other, Exception exception)
        {
            if (parameter.EqualsWithHashCode(other) == false)
                throw exception;
        }

        public static TOut MustBeType<TOut>(this object @object, string parameterName) where TOut : class
        {
            var castedValue = @object as TOut;
            if (castedValue == null)
                throw new TypeMismatchException(parameterName, $"{parameterName} is of type {@object.GetType().FullName} and cannot be downcasted to {typeof(TOut).FullName}.");

            return castedValue;
        }

        [Conditional(Check.PreconditionSymbol)]
        public static void MustHaveValue<T>(this T? parameter, string parameterName) where T : struct
        {
            if (parameter.HasValue == false)
                throw new NullableHasNoValueException(parameterName);
        }

        [Conditional(Check.PreconditionSymbol)]
        public static void MustNotHaveValue<T>(this T? parameter, string parameterName) where T : struct
        {
            if (parameter.HasValue)
                throw new NullableHasValueException(parameterName, parameter.Value);
        }

        [Conditional(Check.PreconditionSymbol)]
        public static void MustBeValidEnumValue<T>(this T parameter, string parameterName)
        {
            var enumType = typeof(T);
            if (Enum.IsDefined(enumType, parameter) == false)
                throw new EnumValueNotDefinedException(parameterName, parameter, enumType);
        }
    }
}