﻿using System;

namespace Light.GuardClauses
{
    /// <summary>
    ///     Defines a range that can be used to check if a specified <see cref="IComparable{T}" /> is in between it or not.
    /// </summary>
    /// <typeparam name="T">The type that the range should be applied to.</typeparam>
    public struct Range<T> where T : IComparable<T>
    {
        /// <summary>
        ///     Gets the lower boundary of the range.
        /// </summary>
        public readonly T From;

        /// <summary>
        ///     Gets the upper boundary of the range.
        /// </summary>
        public readonly T To;

        /// <summary>
        ///     Gets the value indicating whether the From value is included in the range.
        /// </summary>
        public readonly bool IsFromInclusive;

        /// <summary>
        ///     Gets the value indicating whether the To value is included in the range.
        /// </summary>
        public readonly bool IsToInclusive;

        /// <summary>
        ///     Creates a new Range.
        /// </summary>
        /// <param name="from">The lower boundary of the range.</param>
        /// <param name="to">The upper boundary of the range.</param>
        /// <param name="isFromInclusive">The value indicating whether <paramref name="from" /> is part of the range.</param>
        /// <param name="isToInclusive">The value indicating whether <paramref name="to" /> is part of the range.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="to" /> is less than <paramref name="from" />
        ///     (and COMPILE_ASSERTIONS is activated in build settings).
        /// </exception>
        public Range(T from, T to, bool isFromInclusive, bool isToInclusive)
        {
            to.MustNotBeLessThan(from, nameof(to));

            From = from;
            To = to;
            IsFromInclusive = isFromInclusive;
            IsToInclusive = isToInclusive;
        }

        /// <summary>
        ///     Checks if the specified <paramref name="value" /> is within range.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <returns><c>true</c> if value is within range, otherwise <c>false</c>.</returns>
        public bool IsValueWithinRange(T value)
        {
            var expectedLowerBoundaryResult = IsFromInclusive ? 0 : 1;
            var expectedUpperBoundaryResult = IsToInclusive ? 0 : -1;

            var lowerBoundaryCompareResult = value.CompareTo(From);
            if (lowerBoundaryCompareResult < expectedLowerBoundaryResult)
                return false;

            var upperBoundaryCompareResult = value.CompareTo(To);
            if (upperBoundaryCompareResult > expectedUpperBoundaryResult)
                return false;

            return true;
        }

        /// <summary>
        ///     Use this method to create a Range in a fluent style using method chaining. Defines the lower boundary as an
        ///     inclusive value.
        /// </summary>
        /// <param name="value">The value that indicates the inclusive lower boundary of the resulting Range.</param>
        /// <returns>A value you can use to fluently define the upper boundary of a new Range.</returns>
        public static RangeFromInfo FromInclusive(T value)
        {
            return new RangeFromInfo(value, true);
        }

        /// <summary>
        ///     Use this method to create a Range in a fluent style using method chaining. Defines the lower boundary as an
        ///     exclusive value.
        /// </summary>
        /// <param name="value">The value that indicates the exclusive lower boundary of the resulting Range.</param>
        /// <returns>A value you can use to fluently define the upper boundary of a new Range.</returns>
        public static RangeFromInfo FromExclusive(T value)
        {
            return new RangeFromInfo(value, false);
        }

        /// <summary>
        ///     The nested RangeFromInfo can be used to fluently create a Range.
        /// </summary>
        public struct RangeFromInfo
        {
            private readonly T _from;
            private readonly bool _isFromInclusive;

            /// <summary>
            ///     Creates a new RangeFromInfo.
            /// </summary>
            /// <param name="from">The lower boundary of the range.</param>
            /// <param name="isFromInclusive">The value indicating whether <paramref name="from" /> is part of the range.</param>
            public RangeFromInfo(T from, bool isFromInclusive)
            {
                _from = @from;
                _isFromInclusive = isFromInclusive;
            }

            /// <summary>
            ///     Use this method to create a Range in a fluent style using method chaining. Defines the upper boundary as an
            ///     exclusive value.
            /// </summary>
            /// <param name="value">The value that indicates the exclusive upper boundary of the resulting Range.</param>
            /// <returns>A new range with the specified upper and lower boundaries.</returns>
            /// <exception cref="ArgumentOutOfRangeException">
            ///     Thrown when <paramref name="value" /> is less than the lower boundary value
            ///     (and COMPILE_ASSERTIONS is activated in build settings).
            /// </exception>
            public Range<T> ToExclusive(T value)
            {
                return new Range<T>(_from, value, _isFromInclusive, false);
            }

            /// <summary>
            ///     Use this method to create a Range in a fluent style using method chaining. Defines the upper boundary as an
            ///     exclusive value.
            /// </summary>
            /// <param name="value">The value that indicates the inclusive upper boundary of the resulting Range.</param>
            /// <returns>A new range with the specified upper and lower boundaries.</returns>
            /// <exception cref="ArgumentOutOfRangeException">
            ///     Thrown when <paramref name="value" /> is less than the lower boundary value
            ///     (and COMPILE_ASSERTIONS is activated in build settings).
            /// </exception>
            public Range<T> ToInclusive(T value)
            {
                return new Range<T>(_from, value, _isFromInclusive, true);
            }
        }
    }
}