using System;

namespace Light.GuardClauses
{
    public class NullableHasValueException : ArgumentException
    {
        public NullableHasValueException(string parameterName, object actualValue)
            : base($"{parameterName} must have no value, but you specified a Nullable<T> with value {actualValue}.", parameterName)
        {
            
        }
    }
}