// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Web.Facade.Data;
    using Web.Facade.Models;
    using Web.Facade.Services;

    [Authorize(Roles = "client")]
    public class OrderClientHub : Hub
    {
        private readonly IDbContextFactory<OrderDatabaseContext> dbCxtFactory;
        private readonly IUserHubConnectionsRepository connRepo;

        public OrderClientHub(
            IDbContextFactory<OrderDatabaseContext> dbCxtFactory,
            IUserHubConnectionsRepository connRepo)
        {
            this.dbCxtFactory = dbCxtFactory;
            this.connRepo = connRepo;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = this.GetUserId();

            if (this.connRepo.TryAddConnection(userId, this.Context.ConnectionId))
            {
                await using var dbContext = this.dbCxtFactory.CreateDbContext();

                var activeOrders = dbContext.Orders.Where(o =>
                    o.ClientId == userId &&
                    o.Status != OrderStatus.Finished &&
                    o.Status != OrderStatus.Canceled).ToList();

                await this.Clients.User(this.Context.UserIdentifier).SendAsync("Notify", activeOrders);

                await base.OnConnectedAsync();
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (this.connRepo.TryRemoveConnection(this.GetUserId()))
            {
                await base.OnDisconnectedAsync(exception);
            }
        }

        private string GetUserId()
        {
            var actorClaim = this.Context.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor");
            return actorClaim.Value;
        }
    }
}
