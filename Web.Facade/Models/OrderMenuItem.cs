// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Models
{
    public class OrderMenuItem
    {
        public OrderMenuItem()
        {
        }

        public OrderMenuItem(CreateOrderMenuItemDto menuItemDto)
        {
            this.MenuItemId = menuItemDto.MenuItemId;
            this.Amount = menuItemDto.Amount;
        }

        public int OrderId { get; set; }

        public int MenuItemId { get; set; }

        public int Amount { get; set; }
    }
}
