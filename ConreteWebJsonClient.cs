using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using RestClientTemplate.RestClient;
using RestClientTemplate.RestClient.Model;

namespace RestClientTemplate
{
    public class ConreteJsonClient : JsonWebClient
    {
        private readonly RestSettings _settings;
        public ConreteJsonClient(HttpClient client, RestSettings settings) : base(client)
        {
            _settings = settings;
            var baseUrl = $"{settings.BaseUrl.TrimEnd('/')}/";
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.RefreshToken);
            Client.BaseAddress = new Uri(baseUrl);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
        }

        protected override async Task ThrowApiExceptionAsync(HttpResponseMessage response)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            var isServerError = (int)response.StatusCode >= 500;

            if (isServerError)
            {
                throw new ServiceUnavailableException(responseData);
            }

            if (response.Headers.Contains("Token-Expired"))
            {
                throw new TokenExpiredException();
            }

            try
            {
                var errorData = responseData.DeserializeJson<ApiErrorData>();
                throw new ConcreteApiException(response, errorData);
            }
            catch (SerializationException serializeEx)
            {

                var errCode = response.StatusCode.ToString();
                var errMessage = $"{serializeEx.Message}; Api Response: {responseData}";
                var errorData = new ApiErrorData(errMessage, errCode);

                throw new ConcreteApiException(response, errorData);
            }
        }
        public override void SetHeaders(string header)
        {
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
        }

        public override void ResetHeaders()
        {
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this._settings.RefreshToken);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
        }
    }
}