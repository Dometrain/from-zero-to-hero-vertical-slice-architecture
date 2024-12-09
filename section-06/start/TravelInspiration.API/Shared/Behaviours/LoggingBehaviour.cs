using MediatR.Pipeline;

namespace TravelInspiration.API.Shared.Behaviours;

public sealed class LoggingBehaviour<TRequest>(ILogger<TRequest> logger) : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger = logger;

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting feature execution: {featureFromRequestName}",
            typeof(TRequest).Name);
        return Task.CompletedTask;
    }
}
