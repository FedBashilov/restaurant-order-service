// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Notifications.Service.Hubs
{
    using Infrastructure.Auth.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Logging;

    [Authorize(Roles = UserRoles.Cook)]
    public class CookOrderHub : OrderHub
    {
        private readonly IUserHubConnectionsRepository connRepo;
        private readonly ILogger<ClientOrderHub> logger;

        public CookOrderHub(
            IUserHubConnectionsRepository connRepo,
            ILogger<ClientOrderHub> logger)
        {
            this.connRepo = connRepo;
            this.logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = this.GetUserId();

            if (this.connRepo.TryAddConnection(this.Context.ConnectionId, userId))
            {
                await base.OnConnectedAsync();
                this.logger.LogInformation($"Hub connection = {this.Context.ConnectionId} for user = {userId} added successfully.");

                return;
            }

            this.logger.LogWarning($"Failed to add Hub connection = {this.Context.ConnectionId} for user = {userId}.");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            this.connRepo.TryRemoveConnection(this.Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
            this.logger.LogInformation($"Hub connection = {this.Context.ConnectionId} has been removed. Disconnected from Hub.");
        }
    }
}
