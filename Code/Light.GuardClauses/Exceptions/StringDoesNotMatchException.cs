using System;
using System.Text.RegularExpressions;

namespace Light.GuardClauses.Exceptions
{
    public class StringDoesNotMatchException : StringException
    {
        public StringDoesNotMatchException(string parameterName, string actualValue, Regex regularExpression, Exception innerException = null)
            : base($"{parameterName} must match the regular expression {regularExpression}, but you specified {actualValue}.", parameterName, innerException) { }

        public StringDoesNotMatchException(string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException) { }
    }
}