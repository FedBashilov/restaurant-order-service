// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Firebase.Service
{
    using FirebaseAdmin;
    using FirebaseAdmin.Messaging;
    using Google.Apis.Auth.OAuth2;
    using Microsoft.Extensions.Logging;

    public class FbNotificationService : IFbNotificationService
    {
        private readonly IFbTokenService fbTokenService;
        private readonly ILogger<FbNotificationService> logger;

        public FbNotificationService(
            IFbTokenService fbTokenService,
            ILogger<FbNotificationService> logger)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),
                ProjectId = "cybercafe-af556",
            });

            this.fbTokenService = fbTokenService;
            this.logger = logger;
        }

        public async Task SendMessage(string clientId, string payload, string eventType)
        {
            var token = await this.fbTokenService.GetFbToken(clientId);

            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    { "payload", payload },
                    { "event", eventType },
                },
                Token = token,
            };

            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

            this.logger.LogInformation("Sent message to Firebase: " + response, payload);
        }
    }
}