using System;
using System.Diagnostics;

namespace Light.GuardClauses
{
    public static class ComparableGuardClauses
    {
        [Conditional(Guard.PreconditionSymbol)]
        public static void MustNotBeLessThan<T>(this T parameter, T boundary, string parameterName) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) < 0)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than {boundary}, but you specified {parameter}.");
        }

        [Conditional(Guard.PreconditionSymbol)]
        public static void MustNotBeLessThanOrEqualTo<T>(this T parameter, T boundary, string parameterName) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) <= 0)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(Guard.PreconditionSymbol)]
        public static void MustNotBeGreaterThan<T>(this T parameter, T boundary, string parameterName) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) > 0)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than {boundary}, but you specified {parameter}.");
        }

        [Conditional(Guard.PreconditionSymbol)]
        public static void MustNotBeGreaterThanOrEqualTo<T>(this T parameter, T boundary, string parameterName) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) >= 0)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(Guard.PreconditionSymbol)]
        public static void MustBeIn<T>(this T parameter, Range<T> range, string parameterName) where T : IComparable<T>
        {
            var fromBoundaryKind = range.IsFromInclusive ? "inclusive" : "exclusive";
            var toBoundaryKind = range.IsToInclusive ? "inclusive" : "exclusive";

            if (range.IsValueWithinRange(parameter) == false)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must be between {range.From} ({fromBoundaryKind}) and {range.To} ({toBoundaryKind}), but you specified {parameter}.");
        }

        [Conditional(Guard.PreconditionSymbol)]
        public static void MustNotBeIn<T>(this T parameter, Range<T> range, string parameterName) where T : IComparable<T>
        {
            var fromBoundaryKind = range.IsFromInclusive ? "inclusive" : "exclusive";
            var toBoundaryKind = range.IsToInclusive ? "inclusive" : "exclusive";

            if (range.IsValueWithinRange(parameter))
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be between {range.From} ({fromBoundaryKind}) and {range.To} ({toBoundaryKind}), but you specified {parameter}.");
        }
    }
}