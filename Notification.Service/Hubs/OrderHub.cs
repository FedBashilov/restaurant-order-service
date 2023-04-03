// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Notifications.Service.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    public abstract class OrderHub : Hub
    {
        protected string GetUserId()
        {
            var actorClaim = this.Context.User!.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor");
            return actorClaim.Value;
        }
    }
}
