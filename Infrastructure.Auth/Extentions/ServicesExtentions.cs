// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Auth.Extentions
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;

    public static class ServicesExtentions
    {
        public static void AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            var secretKey = configuration.GetSection("JWTSettings:SecretKey").Value ?? throw new NullReferenceException("SecretKey can not be null");
            var issuer = configuration.GetSection("JWTSettings:Issuer").Value ?? throw new NullReferenceException("Issuer can not be null");
            var audience = configuration.GetSection("JWTSettings:Audience").Value ?? throw new NullReferenceException("Audience can not be null");
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromSeconds(10),
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/notifications"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },
                };
            });
        }
    }
}
