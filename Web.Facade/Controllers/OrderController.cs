// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Controllers
{
    using System.Text.Json;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Web.Facade.Exceptions;
    using Web.Facade.Models;
    using Web.Facade.Services;

    [Route("api/orders")]
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
        public async Task<IActionResult> GetAllOrders()
        {
            this.logger.LogInformation($"Starting to get all orders...");
            var orders = await this.orderService.GetAllOrders();

            this.logger.LogInformation($"All orders received successfully! Orders: {JsonSerializer.Serialize(orders)}. Sending the orders in response...");
            return this.Ok(orders);
        }

        [Authorize(Roles = "cook, admin")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id)
        {
            try
            {
                this.logger.LogInformation($"Starting to get order with id = {id} ...");
                var order = await this.orderService.GetOrder(id);

                this.logger.LogInformation($"The order with id = {id} received successfully! order: {JsonSerializer.Serialize(order)}. Sending the order in response...");
                return this.Ok(order);
            }
            catch (NotFoundException ex)
            {
                this.logger.LogWarning(ex, $"Can't get order. Not found order with id = {id}. Sending 404 response...");
                return this.NotFound($"Can't get order. Not found order with id = {id}");
            }
        }

        [Authorize(Roles = "client, cook, admin")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateOrder([FromBody] Order newItem)
        {
            this.logger.LogInformation($"Starting to create order: {JsonSerializer.Serialize(newItem)} ...");
            var order = await this.orderService.CreateOrder(newItem);

            this.logger.LogInformation($"The order created successfully! order: {JsonSerializer.Serialize(order)}. Sending the order in response...");
            return this.StatusCode(201, order);
        }

        [Authorize(Roles = "cook, admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] int id, [FromBody] OrderStatus newStatus)
        {
            try
            {
                this.logger.LogInformation($"Starting to update order status = {newStatus} with id = {id}...");
                var order = await this.orderService.UpdateOrderStatus(id, newStatus);

                this.logger.LogInformation($"The order with id = {id} updated successfully! order: {JsonSerializer.Serialize(order)}. Sending the order in response...");
                return this.Ok(order);
            }
            catch (NotFoundException ex)
            {
                this.logger.LogWarning(ex, $"Can't update order. Not found order with id = {id}. Sending 404 response...");
                return this.NotFound($"Can't update order. Not found order with id = {id}");
            }
        }
    }
}
