// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Auth.Services
{
    using System.IdentityModel.Tokens.Jwt;

    public static class JwtService
    {
        public static string GetClaimValue(string jwtTokenEncoded, string claimType)
        {
            var accessToken = new JwtSecurityToken(jwtTokenEncoded);
            var userRole = accessToken.Claims.First(x => x.Type == claimType).Value;
            return userRole;
        }
    }
}
