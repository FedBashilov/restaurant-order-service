// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using Firebase.Service;
    using Firebase.Service.Models.DTOs;
    using Firebase.Service.Models.Responses;
    using Infrastructure.Auth.Constants;
    using Infrastructure.Auth.Services;
    using Infrastructure.Core.Models.Responses;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/v1/firebase-token")]
    public class FbTokenController : ControllerBase
    {
        private readonly IFbTokenService fbTokenService;
        private readonly ILogger<OrderController> logger;

        public FbTokenController(
            IFbTokenService fbTokenService,
            ILogger<OrderController> logger)
        {
            this.fbTokenService = fbTokenService;
            this.logger = logger;
        }

        [Authorize(Roles = $"{UserRoles.Client}, {UserRoles.Cook}, {UserRoles.Admin}")]
        [HttpPost("")]
        [ProducesResponseType(201, Type = typeof(FbTokenResponse))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> SendFirebaseToken(
            [FromBody] FbTokenDTO fbTokenDTO)
        {
            if (!this.IsInputModelValid(out var message))
            {
                return this.StatusCode(400, new ErrorResponse(message));
            }

            try
            {
                var accessToken = await this.HttpContext.GetTokenAsync("access_token");
                var clientId = JwtService.GetClaimValue(accessToken, "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor");

                await this.fbTokenService.SetFbToken(clientId, fbTokenDTO.FirebaseToken!);

                return this.StatusCode(201, new FbTokenResponse() { ClientId = clientId, FirebaseToken = fbTokenDTO.FirebaseToken });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Can't send Firebase token. {ex.Message}.");
                return this.StatusCode(500, new ErrorResponse($"Can't send Firebase token. Unexpected error."));
            }
        }

        private bool IsInputModelValid([NotNullWhen(false)] out string? errorMessage)
        {
            if (!this.ModelState.IsValid)
            {
                errorMessage = this.ModelState
                    .SelectMany(state => state.Value!.Errors)
                    .Aggregate(string.Empty, (current, error) => current + (error.ErrorMessage + ". "));

                this.logger.LogWarning($"Invalid input parameters. {errorMessage}.");
                return false;
            }

            errorMessage = null;

            return true;
        }
    }
}
