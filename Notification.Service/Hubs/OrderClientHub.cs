// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Notifications.Service.Hubs
{
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Models.Responses;
    using Infrastructure.Database;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Notifications.Service;

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
            var userId = GetUserId();

            if (connRepo.TryAddConnection(this.Context.ConnectionId, userId))
            {
                await using var dbContext = dbCxtFactory.CreateDbContext();

                var activeOrders = dbContext.Orders.Where(o =>
                    o.ClientId == userId &&
                    o.Status != OrderStatus.Closed &&
                    o.Status != OrderStatus.Canceled).ToList();

                var activeOrderResponse = new List<OrderResponse>();
                foreach (var order in activeOrders)
                {
                    activeOrderResponse.Add(new OrderResponse()
                    {
                        Id = order.Id,
                        ClientId = order.ClientId,
                        CreatedDate = order.CreatedDate,
                        CloseDate = order.CloseDate,
                        Status = order.Status.ToString(),
                        TotalPrice = order.TotalPrice,
                        MenuItems = new List<OrderMenuItemResponse>()
                    });
                }

                await this.Clients.Client(this.Context.ConnectionId).SendAsync("Connect", activeOrderResponse);

                await base.OnConnectedAsync();
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            connRepo.TryRemoveConnection(this.Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }

        private string GetUserId()
        {
            var actorClaim = this.Context.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor");
            return actorClaim.Value;
        }
    }
}
