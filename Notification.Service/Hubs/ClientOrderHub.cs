// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Notifications.Service.Hubs
{
    using Infrastructure.Auth.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Notifications.Service;

    [Authorize(Roles = UserRoles.Client)]
    public class ClientOrderHub : OrderHub
    {
        public ClientOrderHub(
            IUserHubConnectionsRepository connRepo)
        {
            this.connRepo = connRepo;
        }
    }
}
