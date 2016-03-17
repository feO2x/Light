using System;
using System.Collections.Generic;

namespace Light.GuardClauses.FrameworkExtensions
{
    public static class Equality
    {
        public const int FirstPrime = 17;
        public const int SecondPrime = 31;

        public static int CreateHashCode<T1, T2>(T1 object1, T2 object2)
        {
            var hash = FirstPrime;
            hash = hash * SecondPrime + object1.GetHashCode();
            hash = hash * SecondPrime + object2.GetHashCode();
            return hash;
        }

        public static int CreateHashCode<T1, T2, T3>(T1 object1, T2 object2, T3 object3)
        {
            var hash = FirstPrime;
            hash = hash * SecondPrime + object1.GetHashCode();
            hash = hash * SecondPrime + object2.GetHashCode();
            hash = hash * SecondPrime + object3.GetHashCode();
            return hash;
        }

        public static int CreateHashCode<T1, T2, T3, T4>(T1 object1, T2 object2, T3 object3, T4 object4)
        {
            var hash = FirstPrime;
            hash = hash * SecondPrime + object1.GetHashCode();
            hash = hash * SecondPrime + object2.GetHashCode();
            hash = hash * SecondPrime + object3.GetHashCode();
            hash = hash * SecondPrime + object4.GetHashCode();
            return hash;
        }

        public static int CreateHashCode<T>(params T[] objects)
        {
            return CreateHashCode((IEnumerable<T>) objects);
        }

        public static int CreateHashCode<T>(IEnumerable<T> objects)
        {
            var hash = FirstPrime;
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var @object in objects)
            {
                hash = hash * SecondPrime + @object.GetHashCode();
            }
            return hash;
        }

        public static bool Compare<T>(T first, T second)
        {
            if (first == null)
                return second == null || second.Equals(null);

            if (second == null)
                return first.Equals(null);

            return first.GetHashCode() == second.GetHashCode() &&
                   first.Equals(second);
        }

        public static bool EqualsWithHashCode<T>(this T reference, T other)
        {
            if (reference == null)
                return other == null || other.Equals(null);

            if (other == null)
                return reference.Equals(null);

            return reference.GetHashCode() == other.GetHashCode() &&
                   reference.Equals(other);
        }

        public static bool EqualsValueWithHashCode<T>(this T value, T other) where T : struct
        {
            return value.GetHashCode() == other.GetHashCode() &&
                   value.Equals(other);
        }

        public static bool EqualsWithHashCode<T>(this IEquatable<T> first, IEquatable<T> second)
        {
            if (first == null)
                return second == null || second.Equals(null);

            if (second == null)
                return first.Equals(null);

            return first.GetHashCode() == second.GetHashCode() &&
                   first.Equals(second);
        }

        public static bool EqualsValueWithHashCode<T>(this IEquatable<T> first, IEquatable<T> second) where T : struct
        {
            return first.GetHashCode() == second.GetHashCode() &&
                   first.Equals(second);
        }

        public static bool EqualsWithHashCode<T>(this IEqualityComparer<T> equalityComparer, T reference, T other)
        {
            return equalityComparer.GetHashCode(reference) == equalityComparer.GetHashCode(other) &&
                   equalityComparer.Equals(reference, other);
        }
    }
}