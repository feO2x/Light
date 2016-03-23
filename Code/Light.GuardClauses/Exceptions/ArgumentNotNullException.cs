using System;

namespace Light.GuardClauses.Exceptions
{
    public class ArgumentNotNullException : ArgumentException
    {
        public ArgumentNotNullException(string parameterName, object actualValue, Exception innerException = null)
            : base($"{parameterName ?? "The specified value "} must be null, but you specified a valid reference to {actualValue}.", parameterName, innerException) { }

        public ArgumentNotNullException(string message, Exception innException = null)
            : base(message, innException) { }
    }
}