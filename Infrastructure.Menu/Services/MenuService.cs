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

        public async Task<List<MenuItem>> GetAllMenu(string accessToken, bool onlyVisible = true)
        {
            return await this.httpRequestFactory.GetHttpRequest<List<MenuItem>>(new Uri($"{this.configuration.Value.Url}/api/v1/menu?onlyVisible={onlyVisible}"), accessToken);
        }

        public async Task<MenuItem> GetMenuItem(int id, string accessToken)
        {
            return await this.httpRequestFactory.GetHttpRequest<MenuItem>(new Uri($"{this.configuration.Value.Url}/api/v1/menu/{id}"), accessToken);
        }

        public async Task<bool> IsMenuItemExist(int menuItemId, string accessToken)
        {
            var menuItem = await GetMenuItem(menuItemId, accessToken);
            return menuItem != null;
        }
    }
}
