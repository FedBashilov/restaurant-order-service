// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Controllers
{
    using System.Text.Json;
    using Infrastructure.Auth.Constants;
    using Infrastructure.Auth.Services;
    using Infrastructure.Core.Extentions;
    using Infrastructure.Core.Models.DTOs;
    using Infrastructure.Core.Models.Responses;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Notifications.Service;
    using Notifications.Service.Hubs;
    using Orders.Service;
    using Orders.Service.Exceptions;

    [Route("api/v1/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        private readonly IHubContext<OrderClientHub> clientHubCtx;
        private readonly IHubContext<OrderCookHub> cookHubCtx;

        private readonly IUserHubConnectionsRepository connRepo;

        private readonly ILogger<OrderController> logger;

        public OrderController(
            IOrderService orderService,
            IHubContext<OrderClientHub> clientHubCtx,
            IHubContext<OrderCookHub> cookHubCtx,
            IUserHubConnectionsRepository connRepo,
            ILogger<OrderController> logger)
        {
            this.orderService = orderService;
            this.clientHubCtx = clientHubCtx;
            this.cookHubCtx = cookHubCtx;
            this.connRepo = connRepo;
            this.logger = logger;
        }

        [Authorize(Roles = $"{UserRoles.Client}, {UserRoles.Cook}, {UserRoles.Admin}")]
        [HttpGet("")]
        [ProducesResponseType(200, Type = typeof(List<OrderResponse>))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetOrders(
            [FromQuery] int offset = 0,
            [FromQuery] int count = 100,
            [FromQuery] bool orderDesc = false,
            [FromQuery] bool onlyActive = false)
        {
            this.logger.LogInformation($"Starting to get all orders...");

            try
            {
                var accessToken = await this.HttpContext.GetTokenAsync("access_token");
                var role = JwtService.GetClaimValue(accessToken, ClaimTypes.Role);
                var clientId = (role == UserRoles.Client) ? JwtService.GetClaimValue(accessToken, ClaimTypes.Actor) : null;

                var orders = await this.orderService.GetOrders(accessToken, offset, count, orderDesc, onlyActive, clientId);

                this.logger.LogInformation($"All orders received successfully! Orders: {JsonSerializer.Serialize(orders)}. Sending the orders in response...");
                return this.Ok(orders);
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, $"Can't get orders. Unexpected error. Sending 500 response...");
                return this.StatusCode(500, new ErrorResponse($"Can't get orders. Unexpected error."));
            }
        }

        [Authorize(Roles = $"{UserRoles.Client}, {UserRoles.Cook}, {UserRoles.Admin}")]
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(OrderResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetOrder(
            [FromRoute] int id)
        {
            this.logger.LogInformation($"Starting to get order with id = {id} ...");

            try
            {
                var accessToken = await this.HttpContext.GetTokenAsync("access_token");

                var role = JwtService.GetClaimValue(accessToken, ClaimTypes.Role);
                var clientId = JwtService.GetClaimValue(accessToken, ClaimTypes.Actor);

                var order = await this.orderService.GetOrder(id, accessToken);
                this.logger.LogInformation($"The order with id = {id} received successfully! order: {JsonSerializer.Serialize(order)}.");

                if (role == UserRoles.Client && order.ClientId != clientId)
                {
                    this.logger.LogInformation($"Client with id {clientId} tried to get someone else's order. Sending 403 response...");
                    return this.Forbid();
                }

                this.logger.LogInformation($"Sending the order in 200 response...");
                return this.Ok(order);
            }
            catch (NotFoundException ex)
            {
                this.logger.LogWarning(ex, $"Can't get order. Not found order with id = {id}. Sending 404 response...");
                return this.NotFound();
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, $"Can't get order. Unexpected error. Sending 500 response...");
                return this.StatusCode(500, new ErrorResponse($"Can't get order. Unexpected error."));
            }
        }

        [Authorize(Roles = UserRoles.Client)]
        [HttpPost("")]
        [ProducesResponseType(201, Type = typeof(OrderResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateOrder(
            [FromBody] CreateOrderDTO newOrder)
        {
            if (!this.IsInputModelValid(out var message))
            {
                return this.StatusCode(400, new ErrorResponse(message));
            }

            this.logger.LogInformation($"Starting to create order: {JsonSerializer.Serialize(newOrder)} ...");

            try
            {
                var accessToken = await this.HttpContext.GetTokenAsync("access_token");
                var clientId = JwtService.GetClaimValue(accessToken, "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor");

                var order = await this.orderService.CreateOrder(newOrder, clientId, accessToken);

                await this.NotifyClientAndCooks(clientId, order);

                this.logger.LogInformation($"The order created successfully! order: {JsonSerializer.Serialize(order)}. Sending the order in response...");
                return this.StatusCode(201, order);
            }
            catch (NotFoundException ex)
            {
                this.logger.LogWarning(ex, $"Can't update order status. {ex.Message}. Sending 400 response...");
                return this.BadRequest($"Menu item does`t exist");
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, $"Can't create order. Unexpected error. Sending 500 response...");
                return this.StatusCode(500, new ErrorResponse($"Can't create order. Unexpected error."));
            }
        }

        [Authorize(Roles = $"{UserRoles.Cook}, {UserRoles.Admin}")]
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(OrderResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateOrderStatus(
            [FromRoute] int id,
            [FromBody] UpdateOrderStatusDTO updateDto)
        {
            if (!this.IsInputModelValid(out var message))
            {
                return this.StatusCode(400, new ErrorResponse(message));
            }

            try
            {
                this.logger.LogInformation($"Starting to update order status = {updateDto.Status} with id = {id}...");
                var accessToken = await this.HttpContext.GetTokenAsync("access_token");

                var order = await this.orderService.UpdateOrderStatus(id, updateDto.Status.ToOrderStatus(), accessToken);

                await this.NotifyClientAndCooks(order.ClientId, order);

                this.logger.LogInformation($"The order with id = {id} updated successfully! order: {JsonSerializer.Serialize(order)}. Sending the order in response...");
                return this.Ok(order);
            }
            catch (ArgumentException ex)
            {
                this.logger.LogWarning(ex, $"Can't update order. {ex.Message}. Sending 400 response...");
                return this.BadRequest(new ErrorResponse($"Can't update order. {ex.Message}."));
            }
            catch (NotFoundException ex)
            {
                this.logger.LogWarning(ex, $"Can't update order status. Not found order with id = {id}. Sending 404 response...");
                return this.NotFound();
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, $"Can't update order status. Unexpected error. Sending 500 response...");
                return this.StatusCode(500, new ErrorResponse($"Can't update order status. Unexpected error."));
            }
        }

        private async Task NotifyClientAndCooks(string clientId, OrderResponse message)
        {
            var clientConnectionIds = this.connRepo.GetConnectionIds(clientId);
            var notifyTasks = new List<Task>();

            foreach (var connectionId in clientConnectionIds)
            {
                notifyTasks.Add(this.clientHubCtx.Clients.Client(connectionId).SendAsync("Notify", message));
            }

            notifyTasks.Add(this.cookHubCtx.Clients.All.SendAsync("Notify", message));

            await Task.WhenAll(notifyTasks);
        }

        private bool IsInputModelValid(out string? errorMessage)
        {
            if (!this.ModelState.IsValid)
            {
                errorMessage = this.ModelState
                    .SelectMany(state => state.Value.Errors)
                    .Aggregate(string.Empty, (current, error) => current + (error.ErrorMessage + ". "));

                return false;
            }

            errorMessage = null;

            return true;
        }
    }
}
