using MediatR;
using TravelInspiration.API.Shared.Domain.Events;

namespace TravelInspiration.API.Shared.DomainEventHandlers;

public sealed class SuggestItineraryStopCreatedEventHandler(
        ILogger<SuggestItineraryStopCreatedEventHandler> logger)
        : INotificationHandler<StopCreatedEvent>
{
    private readonly ILogger<SuggestItineraryStopCreatedEventHandler> _logger = logger;

    public Task Handle(StopCreatedEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listener {listener} to domain event {domainEvent} triggered.",
            GetType().Name,
            notification.GetType().Name);

        // do some AI magic to generate a new itinerary...
        return Task.CompletedTask;
    }
}
