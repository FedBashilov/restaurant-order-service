// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Models.DTOs
{
    public class CreateOrderDTO
    {
        public IEnumerable<CreateOrderMenuItemDTO>? MenuItems { get; set; }
    }
}
