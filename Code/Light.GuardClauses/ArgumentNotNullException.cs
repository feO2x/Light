using System;

namespace Light.GuardClauses
{
    public class ArgumentNotNullException : ArgumentException
    {
        public ArgumentNotNullException(string parameterName, object actualValue, Exception innerException = null)
            : base($"{parameterName} must be null, but you specified a valid reference to {actualValue}.", parameterName, innerException)
        {
            
        }
    }
}