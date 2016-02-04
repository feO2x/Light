
// This file is auto-generated from Guard.NumericalPreconditions.tt
// Do not change its content.

using System;
using System.Diagnostics;

namespace Light.GuardClauses
{
    public partial class Guard
    {
        [Conditional(PreconditionSymbol)]
        public static void IsNotLessThan(this int parameter, int boundary, string parameterName)
        {
            if (parameter < boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void IsNotLessThan(this long parameter, long boundary, string parameterName)
        {
            if (parameter < boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void IsNotLessThan(this short parameter, short boundary, string parameterName)
        {
            if (parameter < boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void IsNotLessThan(this uint parameter, uint boundary, string parameterName)
        {
            if (parameter < boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void IsNotLessThan(this ulong parameter, ulong boundary, string parameterName)
        {
            if (parameter < boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void IsNotLessThan(this ushort parameter, ushort boundary, string parameterName)
        {
            if (parameter < boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void IsNotLessThan(this byte parameter, byte boundary, string parameterName)
        {
            if (parameter < boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void IsNotLessThan(this sbyte parameter, sbyte boundary, string parameterName)
        {
            if (parameter < boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void IsNotLessThan(this decimal parameter, decimal boundary, string parameterName)
        {
            if (parameter < boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void IsNotLessThan(this char parameter, char boundary, string parameterName)
        {
            if (parameter < boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void IsNotLessThan(this double parameter, double boundary, string parameterName)
        {
            if (parameter < boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void IsNotLessThan(this float parameter, float boundary, string parameterName)
        {
            if (parameter < boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThan(int boundary, int parameter, string parameterName)
        {
            if (parameter > boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThan(long boundary, long parameter, string parameterName)
        {
            if (parameter > boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThan(short boundary, short parameter, string parameterName)
        {
            if (parameter > boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThan(uint boundary, uint parameter, string parameterName)
        {
            if (parameter > boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThan(ulong boundary, ulong parameter, string parameterName)
        {
            if (parameter > boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThan(ushort boundary, ushort parameter, string parameterName)
        {
            if (parameter > boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThan(byte boundary, byte parameter, string parameterName)
        {
            if (parameter > boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThan(sbyte boundary, sbyte parameter, string parameterName)
        {
            if (parameter > boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThan(decimal boundary, decimal parameter, string parameterName)
        {
            if (parameter > boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThan(char boundary, char parameter, string parameterName)
        {
            if (parameter > boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThan(double boundary, double parameter, string parameterName)
        {
            if (parameter > boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThan(float boundary, float parameter, string parameterName)
        {
            if (parameter > boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotLessThanOrEqualTo(int boundary, int parameter, string parameterName)
        {
            if (parameter <= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotLessThanOrEqualTo(long boundary, long parameter, string parameterName)
        {
            if (parameter <= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotLessThanOrEqualTo(short boundary, short parameter, string parameterName)
        {
            if (parameter <= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotLessThanOrEqualTo(uint boundary, uint parameter, string parameterName)
        {
            if (parameter <= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotLessThanOrEqualTo(ulong boundary, ulong parameter, string parameterName)
        {
            if (parameter <= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotLessThanOrEqualTo(ushort boundary, ushort parameter, string parameterName)
        {
            if (parameter <= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotLessThanOrEqualTo(byte boundary, byte parameter, string parameterName)
        {
            if (parameter <= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotLessThanOrEqualTo(sbyte boundary, sbyte parameter, string parameterName)
        {
            if (parameter <= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotLessThanOrEqualTo(decimal boundary, decimal parameter, string parameterName)
        {
            if (parameter <= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotLessThanOrEqualTo(char boundary, char parameter, string parameterName)
        {
            if (parameter <= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be less than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThanOrEqualTo(int boundary, int parameter, string parameterName)
        {
            if (parameter >= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThanOrEqualTo(long boundary, long parameter, string parameterName)
        {
            if (parameter >= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThanOrEqualTo(short boundary, short parameter, string parameterName)
        {
            if (parameter >= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThanOrEqualTo(uint boundary, uint parameter, string parameterName)
        {
            if (parameter >= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThanOrEqualTo(ulong boundary, ulong parameter, string parameterName)
        {
            if (parameter >= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThanOrEqualTo(ushort boundary, ushort parameter, string parameterName)
        {
            if (parameter >= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThanOrEqualTo(byte boundary, byte parameter, string parameterName)
        {
            if (parameter >= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThanOrEqualTo(sbyte boundary, sbyte parameter, string parameterName)
        {
            if (parameter >= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThanOrEqualTo(decimal boundary, decimal parameter, string parameterName)
        {
            if (parameter >= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than or equal to {boundary}, but you specified {parameter}.");
        }

        [Conditional(PreconditionSymbol)]
        public static void NotGreaterThanOrEqualTo(char boundary, char parameter, string parameterName)
        {
            if (parameter >= boundary)
                throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must not be greater than or equal to {boundary}, but you specified {parameter}.");
        }

    }
}

