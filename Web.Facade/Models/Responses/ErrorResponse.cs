﻿// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Models.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse(string message)
        {
            this.Message = message;
        }

        public string? Message { get; set; }
    }
}
