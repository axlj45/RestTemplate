namespace RestClientTemplate.RestClient.Model
{
    public class EmptyResponse
    {
        private readonly static EmptyResponse _response = new EmptyResponse();
        public static EmptyResponse Response => _response;
    }
}