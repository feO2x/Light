using System;

namespace Light.GuardClauses.Exceptions
{
    public class EmptyGuidException : ArgumentException
    {
        public EmptyGuidException(string parameterName, Exception innerException = null)
            : base($"{parameterName} must be a valid GUID, but you specified an empty one.", parameterName, innerException)
        {
            
        }
    }
}
