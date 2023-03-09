// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Web.Facade.Data;
    using Web.Facade.Models;

    [Authorize(Roles = "cook")]
    public class OrderCookHub : Hub
    {
        private readonly IDbContextFactory<OrderDatabaseContext> dbCxtFactory;

        public OrderCookHub(IDbContextFactory<OrderDatabaseContext> dbCxtFactory)
        {
            this.dbCxtFactory = dbCxtFactory;
        }

        public override async Task OnConnectedAsync()
        {
            await using var dbContext = this.dbCxtFactory.CreateDbContext();

            var activeOrders = dbContext.Orders.Where(o =>
                o.Status != OrderStatus.Finished &&
                o.Status != OrderStatus.Canceled).ToList();

            await this.Clients.User(this.Context.UserIdentifier).SendAsync("Notify", activeOrders);

            await base.OnConnectedAsync();
        }
    }
}
