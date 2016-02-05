using System;

namespace Light.GuardClauses
{
    public class NullableHasNoValueException : ArgumentException
    {
        public NullableHasNoValueException(string parameterName)
            : base($"{parameterName} must have a value, but you specified a nullable that has none.", parameterName)
        {
            
        }
    }
}