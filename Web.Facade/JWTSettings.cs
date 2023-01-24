// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade
{
    public class JWTSettings
    {
        public string? SecretKey { get; set; }

        public string? Issuer { get; set; }

        public string? Audience { get; set; }
    }
}
