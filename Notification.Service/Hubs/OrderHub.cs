// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Notifications.Service.Hubs
{
    using Infrastructure.Auth.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Notifications.Service;

    public abstract class OrderHub : Hub
    {
        protected IUserHubConnectionsRepository connRepo;

        public override async Task OnConnectedAsync()
        {
            var userId = GetUserId();

            if (this.connRepo.TryAddConnection(this.Context.ConnectionId, userId))
            {
                await base.OnConnectedAsync();
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            this.connRepo.TryRemoveConnection(this.Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }

        private string GetUserId()
        {
            var actorClaim = this.Context.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor");
            return actorClaim.Value;
        }
    }
}
