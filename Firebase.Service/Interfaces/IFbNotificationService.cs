// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Firebase.Service.Interfaces
{
    public interface IFbNotificationService
    {
        public Task SendMessage(string eventType, string clientId, string payload);
    }
}