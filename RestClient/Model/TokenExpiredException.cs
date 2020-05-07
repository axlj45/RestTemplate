using System;

namespace RestClientTemplate.RestClient.Model
{
    [Serializable]
    public class TokenExpiredException : Exception
    {
        public TokenExpiredException() { }
    }
}