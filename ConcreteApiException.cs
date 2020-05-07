using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace RestClientTemplate
{
    [Serializable]
    internal class ConcreteApiException : Exception
    {
        private HttpResponseMessage response;
        private ApiErrorData errorData;

        public ConcreteApiException()
        {
        }

        public ConcreteApiException(string message) : base(message)
        {
        }

        public ConcreteApiException(HttpResponseMessage response, ApiErrorData errorData)
        {
            this.response = response;
            this.errorData = errorData;
        }

        public ConcreteApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConcreteApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}