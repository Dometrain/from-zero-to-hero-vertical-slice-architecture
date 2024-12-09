using MediatR.Pipeline;
using TravelInspiration.API.Shared.Security;

namespace TravelInspiration.API.Shared.Behaviours;

public sealed class LoggingBehaviour<TRequest>(ILogger<TRequest> logger, 
    ICurrentUserService currentUserService) : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger = logger;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting feature execution: {featureFromRequestName}, user {userId}",
            typeof(TRequest).Name, 
            _currentUserService.UserId);
        return Task.CompletedTask;
    }
}
