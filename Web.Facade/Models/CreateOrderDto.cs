// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Models
{
    public class CreateOrderDto
    {
        public string? ClientId { get; set; }

        public IEnumerable<CreateOrderMenuItemDto>? MenuItemIds { get; set; }

        public OrderStatus Status { get; set; }
    }
}
