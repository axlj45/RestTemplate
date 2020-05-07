using System;
using System.Runtime.Serialization;

namespace RestClientTemplate.RestClient
{
    [Serializable]
    public class ServiceUnavailableException : Exception
    {
        public ServiceUnavailableException() { }
        public ServiceUnavailableException(string message) : base(message) { }
        public ServiceUnavailableException(string message, Exception inner)
        : base(message, inner) { }
        protected ServiceUnavailableException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
    }
}