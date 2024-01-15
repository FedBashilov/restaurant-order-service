// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Menu.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Infrastructure.Menu.Interfaces;
    using Infrastructure.Menu.Models;
    using Infrastructure.Menu.Settings;
    using Microsoft.Extensions.Options;

    public class MenuService : IMenuService
    {
        private readonly IHttpRequestFactory httpRequestFactory;
        private readonly IOptions<MenuServiceSettings> configuration;

        public MenuService(
            IHttpRequestFactory httpRequestFactory,
            IOptions<MenuServiceSettings> configuration)
        {
            this.httpRequestFactory = httpRequestFactory;
            this.configuration = configuration;
        }

        public async Task<List<MenuItem>> GetAllMenu(string? accessToken, bool onlyVisible = true)
        {
            return await this.httpRequestFactory.GetHttpRequest<List<MenuItem>>(new Uri($"{this.configuration.Value.Url}/api/v1/menu?onlyVisible={onlyVisible}"), accessToken);
        }

        public async Task<MenuItem> GetMenuItem(int id, string? accessToken)
        {
            return await this.httpRequestFactory.GetHttpRequest<MenuItem>(new Uri($"{this.configuration.Value.Url}/api/v1/menu/{id}"), accessToken);
        }

        public async Task<bool> IsMenuItemExist(int menuItemId, string? accessToken)
        {
            var menuItem = await this.GetMenuItem(menuItemId, accessToken);
            return menuItem != null;
        }
    }
}
