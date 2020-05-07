using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;

namespace RestClientTemplate.RestClient
{
    [Serializable()]
    public class ApiException : Exception
    {
        public ApiException()
        { }

        public ApiException(string message) : base(message)
        { }
        public ApiException(string message, HttpStatusCode statusCode) : base($"{message}. (Status code {(int)statusCode})")
        { }

        public ApiException(string message, Exception innerException) : base(message, innerException)
        { }

        public ApiException(HttpResponseMessage responseMessage) : base($"(Status code {(int)responseMessage.StatusCode})")
        {
            this.ResponseMessage = responseMessage;
        }

        public ApiException(HttpResponseMessage responseMessage, object errorData) : base($"(Status code {(int)responseMessage.StatusCode})")
        {
            this.ResponseMessage = responseMessage;
            this.ResponseError = errorData;
        }

        public HttpResponseMessage ResponseMessage { get; private set; }

        public object ResponseError { get; private set; }

        protected ApiException(SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}