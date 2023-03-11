// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Models.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateOrderStatusDTO
    {
        [Required(ErrorMessage = "The Status param is required")]
        public string? Status { get; set; }
    }
}
