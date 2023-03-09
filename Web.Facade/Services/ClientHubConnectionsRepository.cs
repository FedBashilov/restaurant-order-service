// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    using System.Collections.Concurrent;

    public class ClientHubConnectionsRepository : IUserHubConnectionsRepository
    {
        private readonly ConcurrentDictionary<string, string> connections = new ();

        public bool TryAddConnection(string userId, string connectionId)
        {
            return this.connections.TryAdd(userId, connectionId);
        }

        public bool TryRemoveConnection(string userId)
        {
            return this.connections.TryRemove(userId, out _);
        }

        public string GetConnectionId(string userId)
        {
            if (this.connections.TryGetValue(userId, out var connectionId))
            {
                return connectionId;
            }

            throw new InvalidOperationException($"No connection for client: {userId}");
        }
    }
}
