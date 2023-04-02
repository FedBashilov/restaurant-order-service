// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Notifications.Service.Hubs
{
    using Infrastructure.Auth.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Logging;
    using Notifications.Service;
    using System.Threading.Tasks;

    [Authorize(Roles = UserRoles.Client)]
    public class ClientOrderHub : OrderHub
    {
        public ClientOrderHub(
            IUserHubConnectionsRepository connRepo,
            ILogger<ClientOrderHub> logger)
        {
            this.connRepo = connRepo;
            this.logger = logger;
        }
    }
}
