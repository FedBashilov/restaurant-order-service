// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Menu.Services
{
    using Infrastructure.Menu.Models;

    public interface IMenuService
    {
        public Task<List<MenuItem>> GetAllMenu(string accessToken, bool onlyVisible = true);

        public Task<MenuItem> GetMenuItem(int id, string accessToken);

        public Task<bool> IsMenuItemExist(int menuItemId, string accessToken);
    }
}
