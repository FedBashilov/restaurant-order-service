// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Notifications.Service.Hubs
{
    using Infrastructure.Auth.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Logging;

    [Authorize(Roles = UserRoles.Cook)]
    public class CookOrderHub : OrderHub
    {
        public CookOrderHub(
            IUserHubConnectionsRepository connRepo,
            ILogger<ClientOrderHub> logger)
        {
            this.connRepo = connRepo;
            this.logger = logger;
        }
    }
}
