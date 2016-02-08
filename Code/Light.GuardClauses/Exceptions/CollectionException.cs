using System;

namespace Light.GuardClauses.Exceptions
{
    public class CollectionException : ArgumentException
    {
        public CollectionException(string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException)
        {
            
        }
    }
}
