// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Notifications.Service.Hubs
{
    using Infrastructure.Auth.Constants;
    using Microsoft.AspNetCore.SignalR;

    public abstract class OrderHub : Hub
    {
        protected string GetUserId()
        {
            var actorClaim = this.Context.User!.Claims.First(c => c.Type == ClaimTypes.Actor);
            return actorClaim.Value;
        }
    }
}
