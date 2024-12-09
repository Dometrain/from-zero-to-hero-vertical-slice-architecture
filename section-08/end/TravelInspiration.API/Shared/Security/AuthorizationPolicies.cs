using MediatR.NotificationPublishers;
using Microsoft.AspNetCore.Authorization;

namespace TravelInspiration.API.Shared.Security;

public static class AuthorizationPolicies
{
    public static AuthorizationPolicy HasWriteActionPolicy { get; }
        = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("scope", "write")
            .Build();
}
