using System;

namespace Light.GuardClauses.Exceptions
{
    public class StringException : ArgumentException
    {
        public StringException(string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException)
        {
            
        }
    }
}
