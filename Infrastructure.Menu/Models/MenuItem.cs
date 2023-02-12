// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Menu.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public bool Visible { get; set; } = true;
    }
}
