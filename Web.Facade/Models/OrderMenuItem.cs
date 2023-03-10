// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Models
{
    using Web.Facade.Models.DTOs;

    public class OrderMenuItem
    {
        public OrderMenuItem()
        {
        }

        public OrderMenuItem(CreateOrderMenuItemDTO menuItemDto)
        {
            this.MenuItemId = menuItemDto.MenuItemId;
            this.Amount = menuItemDto.Amount;
        }

        public int OrderId { get; set; }

        public int MenuItemId { get; set; }

        public int Amount { get; set; }
    }
}
