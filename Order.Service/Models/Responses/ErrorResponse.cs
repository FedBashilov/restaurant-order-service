// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Core.Models.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse(string message)
        {
            Message = message;
        }

        public string? Message { get; set; }
    }
}
