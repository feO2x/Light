using System;

namespace Light.GuardClauses.Exceptions
{
    public class EnumValueNotDefinedException : ArgumentException
    {
        public EnumValueNotDefinedException(string parameterName, object actualValue, Type enumType, Exception innerException = null)
            : base($"{parameterName}should be a value of enum {enumType.FullName}, but you specified {actualValue}.", parameterName, innerException)
        {
            
        }
    }
}
