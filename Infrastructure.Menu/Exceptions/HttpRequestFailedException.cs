﻿// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Menu.Exceptions
{
    public class HttpRequestFailedException : Exception
    {
        public HttpRequestFailedException()
        {
        }

        public HttpRequestFailedException(string message)
            : base(message)
        {
        }

        public HttpRequestFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
