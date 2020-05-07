using System.Threading.Tasks;
using AutoMapper;
using RestClientTemplate.Model;
using RestClientTemplate.RestClient;
using RestClientTemplate.RestClient.Model;

namespace RestClientTemplate
{
    public class ConcreteClient
    {
        private readonly IMapper _mapper;
        private readonly IJsonWebClient _client;

        public ConcreteClient(IMapper mapper, IJsonWebClient webClient)
        {
            _mapper = mapper;
            _client = webClient;
        }

        public async Task<ApiCredentials> RefreshTokenAsync()
        {
            ConcreteTokenRequest request = new ConcreteTokenRequest();

            _client.ResetHeaders();
            string resource = $"auth/refresh";

            var response = await _client.PostRefreshTokenAsync<ConcreteTokenRequest, string>(resource, request);

            return new ApiCredentials()
            {
                Token = response
            };
        }

        public async Task<ConcreteItemModel> UpdateItemAsync(string id, ConcreteItemModel item)
        {
            var resource = $"concreteItems/{id}";
            var postItem = _mapper.Map<ConcreteItemPostRequest>(item);

            try
            {
                var result = await _client.PostJsonAsync<ConcreteItemPostRequest, ConcreteItemPostResponse>(resource, postItem);
                return _mapper.Map<ConcreteItemModel>(result);
            }
            catch (TokenExpiredException)
            {
                var newCreds = await RefreshTokenAsync();
                _client.SetHeaders(newCreds.Token);
                var result = await _client.PostJsonAsync<ConcreteItemPostRequest, ConcreteItemPostResponse>(resource, postItem);
                return _mapper.Map<ConcreteItemModel>(result);
            }
        }

        public async Task<ConcreteItemModel> GetItemByIdAsync(string id)
        {
            var resource = $"concreteItems/{id}";

            try
            {
                var result = await _client.GetJsonAsync<ConcreteItemGetResponse>(resource);
                return _mapper.Map<ConcreteItemModel>(result);
            }
            catch (TokenExpiredException)
            {
                var newCreds = await RefreshTokenAsync();
                _client.SetHeaders(newCreds.Token);
                var result = await _client.GetJsonAsync<ConcreteItemGetResponse>(resource);
                return _mapper.Map<ConcreteItemModel>(result);
            }
        }
    }
}