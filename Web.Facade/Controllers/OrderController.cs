// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Controllers
{
    using System.Text.Json;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Web.Facade.Exceptions;
    using Web.Facade.Models;
    using Web.Facade.Services;

    [Route("api/v1/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly ILogger<OrderController> logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            this.orderService = orderService;
            this.logger = logger;
        }

        [Authorize(Roles = "cook, admin")]
        [HttpGet]
        [Route("")]
        [ProducesResponseType(200, Type = typeof(List<OrderResponse>))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetOrders(
            [FromQuery] int offset = 0,
            [FromQuery] int count = 100,
            [FromQuery] bool orderDesc = false)
        {
            this.logger.LogInformation($"Starting to get all orders...");

            try
            {
                var accessToken = await this.HttpContext.GetTokenAsync("access_token");
                var orders = await this.orderService.GetOrders(accessToken, offset, count, orderDesc);

                this.logger.LogInformation($"All orders received successfully! Orders: {JsonSerializer.Serialize(orders)}. Sending the orders in response...");
                return this.Ok(orders);
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, $"Can't get orders. Unexpected error. Sending 500 response...");
                return this.StatusCode(500, new ErrorResponse($"Can't get orders. Unexpected error."));
            }
        }

        [Authorize(Roles = "cook, admin")]
        [HttpGet]
        [Route("{id}")]
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
                var order = await this.orderService.GetOrder(id, accessToken);

                this.logger.LogInformation($"The order with id = {id} received successfully! order: {JsonSerializer.Serialize(order)}. Sending the order in response...");
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

        [Authorize(Roles = "client, cook, admin")]
        [HttpPost]
        [Route("")]
        [ProducesResponseType(201, Type = typeof(OrderResponse))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateOrder(
            [FromBody] CreateOrderDto newOrder)
        {
            this.logger.LogInformation($"Starting to create order: {JsonSerializer.Serialize(newOrder)} ...");

            try
            {
                var accessToken = await this.HttpContext.GetTokenAsync("access_token");
                var order = await this.orderService.CreateOrder(newOrder, accessToken);

                this.logger.LogInformation($"The order created successfully! order: {JsonSerializer.Serialize(order)}. Sending the order in response...");
                return this.StatusCode(201, order);
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, $"Can't create order. Unexpected error. Sending 500 response...");
                return this.StatusCode(500, new ErrorResponse($"Can't create order. Unexpected error."));
            }
        }

        [Authorize(Roles = "cook, admin")]
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(OrderResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateOrderStatus(
            [FromRoute] int id,
            [FromBody] UpdateOrderStatusDto updateDto)
        {
            try
            {
                this.logger.LogInformation($"Starting to update order status = {updateDto.Status} with id = {id}...");
                var accessToken = await this.HttpContext.GetTokenAsync("access_token");
                var order = await this.orderService.UpdateOrderStatus(id, updateDto.Status, accessToken);

                this.logger.LogInformation($"The order with id = {id} updated successfully! order: {JsonSerializer.Serialize(order)}. Sending the order in response...");
                return this.Ok(order);
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
    }
}
