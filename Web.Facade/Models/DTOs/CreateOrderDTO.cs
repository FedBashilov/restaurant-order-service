﻿// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Models.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class CreateOrderDTO
    {
        [Required(ErrorMessage = "The MenuItems param is required")]
        [MinLength(1, ErrorMessage = "The MenuItems param must have at least 1 item")]
        public IEnumerable<CreateOrderMenuItemDTO>? MenuItems { get; set; }
    }
}
