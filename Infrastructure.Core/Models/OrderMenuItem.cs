// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Core.Models
{
    public class OrderMenuItem
    {
        public int OrderId { get; set; }

        public int MenuItemId { get; set; }

        public int Amount { get; set; }
    }
}
