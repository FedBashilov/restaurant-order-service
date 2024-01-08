// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Firebase.Service
{
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;
    using StackExchange.Redis;

    public class FbTokenService : IFbTokenService
    {
        private readonly IDatabase redisDb;
        private readonly ILogger<FbTokenService> logger;

        public FbTokenService(
            ILogger<FbTokenService> logger)
        {
            this.logger = logger;
            this.redisDb = ConnectionMultiplexer.Connect("localhost").GetDatabase();
        }

        public async Task<string> GetFbToken(string clientId)
        {
            return (await this.redisDb.StringGetAsync(clientId)).ToString();
        }

        public async Task SetFbToken(string clientId, string fbToken)
        {
            await this.redisDb.StringSetAsync(clientId, fbToken);
            this.logger.LogInformation("Sent message to Redis: " + clientId, fbToken);
        }
    }
}