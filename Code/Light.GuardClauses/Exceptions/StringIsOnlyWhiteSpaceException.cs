using System;

namespace Light.GuardClauses.Exceptions
{
    public class StringIsOnlyWhiteSpaceException : StringException
    {
        public static StringIsOnlyWhiteSpaceException CreateDefault(string parameterName, string actualValue, Exception innerException = null)
        {
            return new StringIsOnlyWhiteSpaceException($"{parameterName ?? "The value"} must be a string that must have content other than only whitespace, but you specified \"{actualValue}\"", parameterName, innerException);
        }

        public StringIsOnlyWhiteSpaceException(string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException) { }
    }
}