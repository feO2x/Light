using System;

namespace Light.GuardClauses.Exceptions
{
    public class StringIsOnlyWhiteSpaceException : ArgumentException
    {
        public StringIsOnlyWhiteSpaceException(string parameterName, string actualValue, Exception innerException = null)
            : base($"{parameterName} must be a string that must have content other than only whitespace, but you specified \"{actualValue}\"", parameterName, innerException)
        {
            
        }
    }
}
