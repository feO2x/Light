﻿using System;
using System.Diagnostics;

namespace Light.GuardClauses
{
    public static class Guard
    {
        /// <summary>
        /// The compiler symbol that must be added so that calls to this class are compiled into the assembly.
        /// This value is "COMPILE_PRECONDITIONS".
        /// </summary>
        public const string PreconditionSymbol = "COMPILE_PRECONDITIONS";

        [Conditional(PreconditionSymbol)]
        public static void MustNotBeNull<T>(this T parameter, string parameterName) where T : class
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName);
        }

        [Conditional(PreconditionSymbol)]
        public static void MustNotBeLessThan<T>(this T parameter, T boundary, string parameterName) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) < 0)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotLessThanOrEqualTo<T>(T boundary, T parameter, string parameterName) where T : IComparable<T>
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
        public static void NotGreaterThanOrEqualTo<T>(T boundary, T parameter, string parameterName) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) >= 0)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotLessOrEqualTo(int boundary, int parameter, string paramterName)
        {
            if (parameter <= boundary)
                throw new ArgumentOutOfRangeException(paramterName, parameter, $"{paramterName} must not be less or equal to ");
        }

        [Conditional(PreconditionSymbol)]
        public static void In<T>(Range<T> range, T parameter, string parameterName) where T : IComparable<T>
        {
            var fromBoundaryKind = range.IsFromInclusive ? "inclusive" : "exclusive";
            var toBoundaryKind = range.IsToInclusive ? "inclusive" : "exclusive";

            if (range.CheckValue(parameter))
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} shout be between {range.From} ({fromBoundaryKind}) and {range.To} ({toBoundaryKind}), but you specified {parameter}.");
        }
    }
}
