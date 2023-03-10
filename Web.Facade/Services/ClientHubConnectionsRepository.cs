// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    public class ClientHubConnectionsRepository : IUserHubConnectionsRepository
    {
        private readonly Dictionary<string, string> connections = new ();

        public bool TryAddConnection(string connectionId, string userId)
        {
            return this.connections.TryAdd(connectionId, userId);
        }

        public bool TryRemoveConnection(string connectionId)
        {
            return this.connections.Remove(connectionId, out _);
        }

        public IEnumerable<string> GetConnectionIds(string userId)
        {
            var connnectionIds = this.connections.Where(x => x.Value == userId).Select(x => x.Key);

            if (connnectionIds.Any())
            {
                return connnectionIds;
            }

            throw new InvalidOperationException($"No connection for client: {userId}");
        }
    }
}
