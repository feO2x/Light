using System;

namespace Light.GuardClauses.Exceptions
{
    public class TypeMismatchException : ArgumentException
    {
        public TypeMismatchException(string parameterName, string message, Exception innerException = null)
            : base(message, parameterName, innerException)
        {
            
        }
    }
}
