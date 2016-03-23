﻿using System;

namespace Light.GuardClauses.Exceptions
{
    public class NullableHasNoValueException : ArgumentException
    {
        public NullableHasNoValueException(string parameterName)
            : base($"{parameterName} must have a value, but you specified a nullable that has none.", parameterName) { }

        public NullableHasNoValueException(string message, string parameterName)
            : base(message, parameterName) { }
    }
}