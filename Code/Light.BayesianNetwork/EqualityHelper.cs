using System;
using System.Collections.Generic;

namespace Light.BayesianNetwork
{
    public sealed class EqualityHelper
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
            return CreateHashCode((IEnumerable<T>)objects);
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

        public static bool CompareNullableValues<T>(T first, T second) where T : class, IEquatable<T>
        {
            if (first == null)
                return second == null || second.Equals(null);

            if (second == null)
                return first.Equals(null);

            return first.GetHashCode() == second.GetHashCode() &&
                   first.Equals(second);
        }

        public static bool CompareNonNullableValues<T>(T first, T second) where T : struct, IEquatable<T>
        {
            return first.GetHashCode() == second.GetHashCode() &&
                   first.Equals(second);
        }
    }
}