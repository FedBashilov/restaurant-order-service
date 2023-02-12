// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Models
{
    public class CreateOrderDto
    {
        public IEnumerable<CreateOrderMenuItemDto>? MenuItemIds { get; set; }

        public OrderStatus Status { get; set; }
    }
}
