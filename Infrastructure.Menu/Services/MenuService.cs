// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Menu.Services
{
    using Infrastructure.Menu.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MenuService : IMenuService
    {
        private readonly IHttpRequestFactory httpRequestFactory;

        public MenuService(IHttpRequestFactory httpRequestFactory)
        {
            this.httpRequestFactory = httpRequestFactory;
        }

        public async Task<List<MenuItem>> GetAllMenu(string accessToken)
        {
            return await this.httpRequestFactory.GetHttpRequest<List<MenuItem>>(new Uri("http://localhost:5291/api/v1/menu"), accessToken);
        }

        public async Task<MenuItem> GetMenuItem(int id, string accessToken)
        {
            return await this.httpRequestFactory.GetHttpRequest<MenuItem>(new Uri($"http://localhost:5291/api/v1/menu/{id}"), accessToken);

        }
    }
}
