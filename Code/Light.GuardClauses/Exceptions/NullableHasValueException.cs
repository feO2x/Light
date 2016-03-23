using System;

namespace Light.GuardClauses.Exceptions
{
    public class NullableHasValueException : ArgumentException
    {
        public NullableHasValueException(string parameterName, object actualValue)
            : base($"{parameterName} must have no value, but you specified a Nullable<T> with value {actualValue}.", parameterName) { }

        public NullableHasValueException(string message, string parameterName) : base(message, parameterName) { }
    }
}