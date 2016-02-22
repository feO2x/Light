using System;

namespace Light.Serialization
{
    public class DeserializationException : Exception
    {
        public DeserializationException(string message, Exception innerException = null)
            : base(message, innerException)
        {
            
        }
    }
}