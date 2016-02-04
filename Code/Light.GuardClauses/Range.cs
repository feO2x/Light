using System;

namespace Light.GuardClauses
{
    public struct Range<T> where T : IComparable<T>
    {
        public readonly T From;
        public readonly T To;
        public readonly bool IsFromInclusive;
        public readonly bool IsToInclusive;

        public Range(T from, T to, bool isFromInclusive, bool isToInclusive)
        {
            to.IsNotLessThan(from, nameof(to));

            From = from;
            To = to;
            IsFromInclusive = isFromInclusive;
            IsToInclusive = isToInclusive;
        }

        public bool CheckValue(T value)
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

        public static RangeFromInfo FromInclusive(T value)
        {
            return new RangeFromInfo(value, true);
        }

        public static RangeFromInfo FromExclusive(T value)
        {
            return new RangeFromInfo(value, false);
        }

        public struct RangeFromInfo
        {
            private readonly T _from;
            private readonly bool _isFromInclusive;

            public RangeFromInfo(T from, bool isFromInclusive)
            {
                _from = @from;
                _isFromInclusive = isFromInclusive;
            }

            public Range<T> ToExclusive(T value)
            {
                return new Range<T>(_from, value, _isFromInclusive, false);
            }

            public Range<T> ToInclusive(T value)
            {
                return new Range<T>(_from, value, _isFromInclusive, true);
            }
        }
    }
}