using System;
using System.Net.Http;
using System.Threading.Tasks;
using RestClientTemplate.RestClient.Model;

namespace RestClientTemplate.RestClient
{
    public abstract class JsonWebClient : IJsonWebClient
    {
        protected readonly HttpClient Client;

        protected JsonWebClient(HttpClient client)
        {
            Client = client;
        }

        public async Task<TResponse> PostRefreshTokenAsync<TPost, TResponse>(string endpoint, TPost input) where TResponse : class
            => await ExecuteAsync<TResponse>(() => Client.PostAsync(endpoint, new JsonContent(input)));
        public async Task<TResponse> PostJsonAsync<TPost, TResponse>(string endpoint, TPost input) where TResponse : class
            => await ExecutePostAsync<TResponse>(() => Client.PostAsync(endpoint, new JsonContent(input)));
        public async Task<TResponse> PutJsonAsync<TPost, TResponse>(string endpoint, TPost input) where TResponse : class
            => await ExecuteAsync<TResponse>(() => Client.PutAsync(endpoint, new JsonContent(input)));
        public async Task<TResponse> GetByteAsync<TResponse>(string endpoint) where TResponse : class
            => await ExecuteByteAsync<TResponse>(() => Client.GetAsync(endpoint));
        public async Task<TResponse> GetJsonAsync<TResponse>(string endpoint) where TResponse : class
            => await ExecuteAsync<TResponse>(() => Client.GetAsync(endpoint));

        private async Task<TResponse> ExecuteAsync<TResponse>(Func<Task<HttpResponseMessage>> request) where TResponse : class
        {
            var data = await ExecuteAsync(request);

            if (typeof(TResponse) == typeof(EmptyResponse))
                return EmptyResponse.Response as TResponse;

            if (typeof(TResponse) == typeof(string))
                return data as TResponse;

            try
            {
                return data.DeserializeJson<TResponse>();
            }
            catch (Exception)
            {
                throw new ApiException($"Unable to deserialize response: {data}");
            }
        }   

        private async Task<string> ExecuteAsync(Func<Task<HttpResponseMessage>> request)
        {
            try
            {
                var response = await request();
                if (!response.IsSuccessStatusCode)
                {
                    await ThrowApiExceptionAsync(response);
                }
                var data = await response.Content.ReadAsStringAsync();
                return data;
            }
            catch (HttpRequestException requestException)
            {
                throw new ServiceUnavailableException($"An error occurred while trying to access the service.  See inner exception.", requestException);
            }
            catch (TaskCanceledException asyncEx)
            {
                throw new ServiceUnavailableException($"The operation timed out while trying to access the service.  See inner exception.", asyncEx);
            }
            catch (OperationCanceledException opEx)
            {
                throw new ServiceUnavailableException($"The operation timed out while trying to access the service.  See inner exception.", opEx);
            }
        }

        private async Task<TResponse> ExecutePostAsync<TResponse>(Func<Task<HttpResponseMessage>> request) where TResponse : class
        {
            var data = await ExecutePostAsync(request);

            if (typeof(TResponse) == typeof(EmptyResponse))
                return EmptyResponse.Response as TResponse;

            return data as TResponse;
        }

        private async Task<ApiCredentials> ExecutePostAsync(Func<Task<HttpResponseMessage>> request)
        {
            try
            {
                var response = await request();
                if (!response.IsSuccessStatusCode)
                {
                    await ThrowApiExceptionAsync(response);
                }
                
                var data = await response.Content.ReadAsStringAsync();

                return data.DeserializeJson<ApiCredentials>();
            }
            catch (HttpRequestException requestException)
            {
                throw new ServiceUnavailableException($"An error occurred while trying to access the service.  See inner exception.", requestException);
            }
            catch (TaskCanceledException asyncEx)
            {
                throw new ServiceUnavailableException($"The operation timed out while trying to access the service.  See inner exception.", asyncEx);
            }
            catch (OperationCanceledException opEx)
            {
                throw new ServiceUnavailableException($"The operation timed out while trying to access the service.  See inner exception.", opEx);
            }
        }

        private async Task<TResponse> ExecuteByteAsync<TResponse>(Func<Task<HttpResponseMessage>> request) where TResponse : class
        {
            var data = await ExecuteByteAsync(request);

            if (typeof(TResponse) == typeof(EmptyResponse))
                return EmptyResponse.Response as TResponse;
            else
                return data as TResponse;
        }
        private async Task<byte[]> ExecuteByteAsync(Func<Task<HttpResponseMessage>> request)
        {
            try
            {
                var response = await request();
                if (!response.IsSuccessStatusCode)
                {
                    await ThrowApiExceptionAsync(response);
                }

                var data = await response.Content.ReadAsByteArrayAsync();
                return data;
            }
            catch (HttpRequestException requestException)
            {
                throw new ServiceUnavailableException($"An error occurred while trying to access the service.  See inner exception.", requestException);
            }
            catch (TaskCanceledException asyncEx)
            {
                throw new ServiceUnavailableException($"The operation timed out while trying to access the service.  See inner exception.", asyncEx);
            }
            catch (OperationCanceledException opEx)
            {
                throw new ServiceUnavailableException($"The operation timed out while trying to access the service.  See inner exception.", opEx);
            }
        }

        public abstract void SetHeaders(string header);

        public abstract void ResetHeaders();

        protected abstract Task ThrowApiExceptionAsync(HttpResponseMessage response);
    }
}