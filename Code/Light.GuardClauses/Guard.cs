using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Light.GuardClauses
{
    public static class Guard
    {
        /// <summary>
        /// The compiler symbol that must be added so that calls to this class are compiled into the assembly.
        /// This value is "COMPILE_PRECONDITIONS" (without quotation marks).
        /// </summary>
        public const string PreconditionSymbol = "COMPILE_PRECONDITIONS";

        [Conditional(PreconditionSymbol)]
        public static void Against(bool preconditionResult, Func<Exception> createException)
        {
            if (preconditionResult)
                throw createException();
        }

        [Conditional(PreconditionSymbol)]
        public static void MustNotBeNull(this object parameter, string parameterName)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName);
        }

        [Conditional(PreconditionSymbol)]
        public static void MustBeNull(this object parameter, string parameterName)
        {
            if (parameter != null)
                throw new ArgumentNotNullException(parameterName, parameter);
        }

        [Conditional(PreconditionSymbol)]
        public static void MustNotBeLessThan<T>(this T parameter, T boundary, string parameterName) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) < 0)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void MustNotBeLessThanOrEqualTo<T>(this T parameter, T boundary, string parameterName) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) <= 0)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void MustNotBeGreaterThan<T>(this T parameter, T boundary, string parameterName) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) > 0)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void MustNotBeGreaterThanOrEqualTo<T>(this T parameter, T boundary, string parameterName) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) >= 0)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void MustBeIn<T>(this T parameter, Range<T> range, string parameterName) where T : IComparable<T>
        {
            var fromBoundaryKind = range.IsFromInclusive ? "inclusive" : "exclusive";
            var toBoundaryKind = range.IsToInclusive ? "inclusive" : "exclusive";

            if (range.IsValueWithinRange(parameter) == false)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must be between {range.From} ({fromBoundaryKind}) and {range.To} ({toBoundaryKind}), but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void MustNotBeIn<T>(this T parameter, Range<T> range, string parameterName) where T : IComparable<T>
        {
            var fromBoundaryKind = range.IsFromInclusive ? "inclusive" : "exclusive";
            var toBoundaryKind = range.IsToInclusive ? "inclusive" : "exclusive";

            if (range.IsValueWithinRange(parameter))
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be between {range.From} ({fromBoundaryKind}) and {range.To} ({toBoundaryKind}), but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void MustBeOneOf<T>(this T parameter, IReadOnlyList<T> items, string parameterName)
        {
            if (items.Contains(parameter))
                return;

            var stringBuilder = new StringBuilder().AppendItems(items);
            throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must be one of the items ({stringBuilder}), but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void MustNotBeOneOf<T>(this T parameter, IReadOnlyList<T> items, string parameterName)
        {
            if (items.Contains(parameter) == false)
                return;

            var stringBuilder = new StringBuilder().AppendItems(items);
            throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must be none of the items ({stringBuilder}), but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void MustNotBeNullOrEmpty<T>(this IReadOnlyCollection<T> collection, string parameterName)
        {
            if (collection == null)
                throw new ArgumentNullException(parameterName);

            if (collection.Count == 0)
                throw new EmptyCollectionException(parameterName);
        }

        [Conditional(PreconditionSymbol)]
        public static void MustNotBeNullOrEmpty(this string @string, string parameterName)
        {
            if (@string == null)
                throw new ArgumentNullException(parameterName);

            if (@string == string.Empty)
                throw new EmptyStringException(parameterName);
        }

        public static TOut MustBeType<TOut>(this object @object, string parameterName) where TOut : class
        {
            var castedValue = @object as TOut;
            if (castedValue == null)
                throw new TypeMismatchException(parameterName, $"{parameterName} is of type {@object.GetType().FullName} and cannot be downcasted to {typeof(TOut).FullName}.");

            return castedValue;
        }

        [Conditional(PreconditionSymbol)]
        public static void MustHaveValue<T>(this T? parameter, string parameterName) where T : struct
        {
            if (parameter.HasValue == false)
                throw new NullableHasNoValueException(parameterName);
        }

        [Conditional(PreconditionSymbol)]
        public static void MustNotHaveValue<T>(this T? parameter, string parameterName) where T : struct
        {
            if (parameter.HasValue)
                throw new NullableHasValueException(parameterName, parameter.Value);
        }

        [Conditional(PreconditionSymbol)]
        public static void MustMatch(this string parameter, Regex pattern, string parameterName)
        {
            var match = pattern.Match(parameter);
            if (match.Success == false)
                throw new StringDoesNotMatchException(parameterName, parameter, pattern);
        }

        [Conditional(PreconditionSymbol)]
        public static void MustNotBeEmpty(this Guid parameter, string parameterName)
        {
            if (parameter == Guid.Empty)
                throw new EmptyGuidException(parameterName);
        }

        [Conditional(PreconditionSymbol)]
        public static void MustBeValidEnumValue<T>(this T parameter, string parameterName)
        {
            var enumType = typeof (T);
            if (Enum.IsDefined(enumType, parameter) == false)
                throw new EnumValueNotDefinedException(parameterName, parameter, enumType);
        }

        [Conditional(PreconditionSymbol)]
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

        [Conditional(PreconditionSymbol)]
        public static void MustContain(this string parameter, string containedText, string parameterName)
        {
            if (parameter.Contains(containedText) == false)
                throw new StringException($"{parameterName} must contain the text \"{containedText}\", but you specified \"{parameter}\".", parameterName);
        }
    }
}
