// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Firebase.Service
{
    using Firebase.Service.Interfaces;
    using Firebase.Service.Settings;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using StackExchange.Redis;

    public class FbTokenService : IFbTokenService
    {
        private readonly IDatabase redisDb;
        private readonly RedisSettings redisSettings;
        private readonly ILogger<FbTokenService> logger;

        public FbTokenService(
            IOptions<RedisSettings> redisSettings,
            ILogger<FbTokenService> logger)
        {
            this.redisSettings = redisSettings.Value;
            this.logger = logger;

            try
            {
                this.redisDb = ConnectionMultiplexer.Connect(this.redisSettings.HostName!).GetDatabase();
            }
            catch
            {
                this.logger.LogError("Redis connection failed!");
            }
        }

        public async Task<string> GetFbToken(string clientId)
        {
            return (await this.redisDb.StringGetAsync(clientId)).ToString();
        }

        public async Task SetFbToken(string clientId, string fbToken)
        {
            await this.redisDb.StringSetAsync(clientId, fbToken, TimeSpan.FromDays(60));
            this.logger.LogInformation("Sent message to Redis: " + clientId, fbToken);
        }
    }
}