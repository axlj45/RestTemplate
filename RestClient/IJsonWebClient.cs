using System.Threading.Tasks;

namespace RestClientTemplate.RestClient
{
    public interface IJsonWebClient
    {
        Task<TResponse> PutJsonAsync<TPost, TResponse>(string endpoint, TPost input) where TResponse : class;
        Task<TResponse> PostJsonAsync<TPost, TResponse>(string endpoint, TPost input) where TResponse : class;
        Task<TResponse> PostRefreshTokenAsync<TPost, TResponse>(string endpoint, TPost input) where TResponse : class;
        Task<TResponse> GetByteAsync<TResponse>(string endpoint) where TResponse : class;
        Task<TResponse> GetJsonAsync<TResponse>(string endpoint) where TResponse : class;
        void SetHeaders(string header);
        void ResetHeaders();
    }
}
