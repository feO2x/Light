using System;
using System.Diagnostics;

namespace Light.GuardClauses
{
    public static class Check
    {
        /// <summary>
        ///     The compiler symbol that must be added so that calls to this class are compiled into the assembly.
        ///     This value is "COMPILE_PRECONDITIONS" (without quotation marks).
        /// </summary>
        public const string CompileAssertionsSymbol = "COMPILE_ASSERTIONS";

        /// <summary>
        /// Checks that the specified <param name="assertionResult"></param> is true, otherwise throws the specified exception.
        /// </summary>
        /// <param name="assertionResult">The result of an assertions to be checked.</param>
        /// <param name="otherwiseCreateException">The delegate that creates the exception to be thrown.</param>
        [Conditional(CompileAssertionsSymbol)]
        public static void That(bool assertionResult, Func<Exception> otherwiseCreateException)
        {
            if (assertionResult == false)
                throw otherwiseCreateException();
        }

        /// <summary>
        /// Checks that the specified <param name="assertionResult"></param> is false, otherwise throws the specified exception.
        /// </summary>
        /// <param name="assertionResult">The result of an assertion to be checked.</param>
        /// <param name="createException">The delegate that creates the exception to be thrown.</param>
        [Conditional(CompileAssertionsSymbol)]
        public static void Against(bool assertionResult, Func<Exception> createException)
        {
            if (assertionResult)
                throw createException();
        }
    }
}