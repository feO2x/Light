using System;

namespace Light.GuardClauses.Exceptions
{
    public class EmptyGuidException : ArgumentException
    {
        public EmptyGuidException(string parameterName, Exception innerException = null)
            : base($"{parameterName ?? "The value"} must be a valid GUID, but you specified an empty one.", parameterName, innerException) { }

        public EmptyGuidException(string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException) { }
    }
}