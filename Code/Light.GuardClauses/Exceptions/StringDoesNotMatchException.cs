using System;
using System.Text.RegularExpressions;

namespace Light.GuardClauses.Exceptions
{
    public class StringDoesNotMatchException : ArgumentException
    {
        public StringDoesNotMatchException(string parameterName, string actualValue, Regex regularExpression, Exception innException = null)
            : base($"{parameterName} must match the regular expression {regularExpression}, but you specified {actualValue}.", parameterName, innException)
        {
            
        }
    }
}
