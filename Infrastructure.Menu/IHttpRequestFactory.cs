// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Menu
{
    public interface IHttpRequestFactory
    {
        public Task<T> GetHttpRequest<T>(Uri url, string? accessToken = default);

        public Task<T> PostHttpRequest<T>(Uri url, string serializedBody, string? accessToken = default);

        public Task<T> PutHttpRequest<T>(Uri url, string serializedBody, string? accessToken = default);

        public Task DeleteHttpRequest(Uri url, string? accessToken = default);
    }
}
