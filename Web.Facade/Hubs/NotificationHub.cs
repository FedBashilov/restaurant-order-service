// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    public class NotificationHub : Hub
    {
        public Task SendMessage(string message)
        {
            return this.Clients.Others.SendAsync("Send", message);
        }
    }
}
