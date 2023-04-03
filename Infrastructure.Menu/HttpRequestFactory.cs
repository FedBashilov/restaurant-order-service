// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Menu
{
    using System.Net.Http.Headers;
    using System.Text.Json;
    using Infrastructure.Menu.Exceptions;

    internal class HttpRequestFactory : IHttpRequestFactory
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HttpRequestFactory(IHttpClientFactory httpClientFactory) =>
            this.httpClientFactory = httpClientFactory;

        public async Task<T> GetHttpRequest<T>(Uri url, string? accessToken = default)
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                url);
            if (accessToken != default)
            {
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var httpClient = this.httpClientFactory.CreateClient();

            using var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            CheckResponseMessage(httpResponseMessage);

            var contentString = await httpResponseMessage.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<T>(contentString, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });

            return response!;
        }

        public async Task<T> PostHttpRequest<T>(Uri url, string serializedBody, string? accessToken = default)
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Post,
                url);
            if (accessToken != default)
            {
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            httpRequestMessage.Content = new StringContent(serializedBody);

            var httpClient = this.httpClientFactory.CreateClient();

            using var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            CheckResponseMessage(httpResponseMessage);

            var contentString = await httpResponseMessage.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<T>(contentString, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });

            return response!;
        }

        public async Task<T> PutHttpRequest<T>(Uri url, string serializedBody, string? accessToken = default)
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Put,
                url);
            if (accessToken != default)
            {
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            httpRequestMessage.Content = new StringContent(serializedBody);

            var httpClient = this.httpClientFactory.CreateClient();

            using var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            CheckResponseMessage(httpResponseMessage);

            var contentString = await httpResponseMessage.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<T>(contentString, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });

            return response!;
        }

        public async Task DeleteHttpRequest(Uri url, string? accessToken = default)
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Delete,
                url);
            if (accessToken != default)
            {
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var httpClient = this.httpClientFactory.CreateClient();

            using var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            CheckResponseMessage(httpResponseMessage);
        }

        private static void CheckResponseMessage(HttpResponseMessage httpResponseMessage)
        {
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new HttpRequestFailedException($"Status code: {httpResponseMessage.StatusCode}. Message: {httpResponseMessage.Content}");
            }
        }
    }
}
