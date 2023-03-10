// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Menu.Services
{
    using Infrastructure.Menu.Models;
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MenuService : IMenuService
    {
        private readonly IHttpRequestFactory httpRequestFactory;
        private readonly IOptions<MenuConfig> configuration;

        public MenuService(
            IHttpRequestFactory httpRequestFactory,
            IOptions<MenuConfig> configuration)
        {
            this.httpRequestFactory = httpRequestFactory;
            this.configuration = configuration;
        }

        public async Task<List<MenuItem>> GetAllMenu(string accessToken)
        {
            return await this.httpRequestFactory.GetHttpRequest<List<MenuItem>>(new Uri($"{this.configuration.Value.Url}/api/v1/menu"), accessToken);
        }

        public async Task<MenuItem> GetMenuItem(int id, string accessToken)
        {
            return await this.httpRequestFactory.GetHttpRequest<MenuItem>(new Uri($"{this.configuration.Value.Url}/api/v1/menu/{id}"), accessToken);
        }
    }
}
