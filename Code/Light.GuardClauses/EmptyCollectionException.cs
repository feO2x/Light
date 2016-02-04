using System;

namespace Light.GuardClauses
{
    public class EmptyCollectionException : ArgumentException
    {
        public EmptyCollectionException(string parameterName, Exception innerException = null)
            : base($"{parameterName} must not be an empty collection, but you specified one.", parameterName, innerException)
        {
        }
    }
}