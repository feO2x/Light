using System;

namespace Light.GuardClauses.Exceptions
{
    public class EmptyStringException : StringException
    {
        public EmptyStringException(string parameterName, Exception innerException = null)
            : base($"{parameterName} must not be an empty string, but you specified one.", parameterName, innerException)
        {
            
        }
    }
}
