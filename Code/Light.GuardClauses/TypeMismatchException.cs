using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.GuardClauses
{
    public class TypeMismatchException : ArgumentException
    {
        public TypeMismatchException(string parameterName, string message, Exception innerException = null)
            : base(message, parameterName, innerException)
        {
            
        }
    }
}
