// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Notifications.Service.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;
    using Notifications.Service;

    public abstract class OrderHub : Hub
    {
        protected IUserHubConnectionsRepository connRepo;
        protected ILogger<ClientOrderHub> logger;

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

        private string GetUserId()
        {
            var actorClaim = this.Context.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor");
            return actorClaim.Value;
        }
    }
}
