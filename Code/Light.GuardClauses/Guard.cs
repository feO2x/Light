using System;
using System.Diagnostics;

namespace Light.GuardClauses
{
    public static class Guard
    {
        /// <summary>
        ///     The compiler symbol that must be added so that calls to this class are compiled into the assembly.
        ///     This value is "COMPILE_PRECONDITIONS" (without quotation marks).
        /// </summary>
        public const string PreconditionSymbol = "COMPILE_PRECONDITIONS";

        [Conditional(PreconditionSymbol)]
        public static void Against(bool preconditionResult, Func<Exception> createException)
        {
            if (preconditionResult)
                throw createException();
        }
    }
}