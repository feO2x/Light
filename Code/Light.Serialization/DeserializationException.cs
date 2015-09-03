using System;

namespace Light.Serialization
{
    public class DeserializationException : Exception
    {
        public DeserializationException(string message)
            : base(message)
        {
            
        }
    }
}