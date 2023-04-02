// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Notifications.Service.Hubs
{
    using Infrastructure.Auth.Constants;
    using Microsoft.AspNetCore.Authorization;

    [Authorize(Roles = UserRoles.Cook)]
    public class CookOrderHub : OrderHub
    {
        public CookOrderHub(
            IUserHubConnectionsRepository connRepo)
        {
            this.connRepo = connRepo;
        }
    }
}
