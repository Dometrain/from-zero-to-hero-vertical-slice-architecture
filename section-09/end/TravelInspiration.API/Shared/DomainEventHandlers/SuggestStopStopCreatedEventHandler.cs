using MediatR;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Domain.Events;
using TravelInspiration.API.Shared.Persistence;

namespace TravelInspiration.API.Shared.DomainEventHandlers;

public sealed class SuggestStopStopCreatedEventHandler(
        ILogger<SuggestStopStopCreatedEventHandler> logger,
        TravelInspirationDbContext dbContext)
        : INotificationHandler<StopCreatedEvent>
{
    private readonly ILogger<SuggestStopStopCreatedEventHandler> _logger = logger;
    private readonly TravelInspirationDbContext _dbContext = dbContext;

    public Task Handle(StopCreatedEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listener {listener} to domain event {domainEvent} triggered.",
            GetType().Name,
            notification.GetType().Name);

        var incomingStop = notification.Stop;

        // do some AI magic to generate a new stop...

        var stop = new Stop($"AI-ified stop based on {incomingStop.Name}")
        {
            ItineraryId = incomingStop.ItineraryId,
            ImageUri = new Uri("https://herebeimages.ciom/aigeneratedimage.png"),
            Suggested = true
        };

        _dbContext.Stops.Add(stop);
        return Task.CompletedTask;
    }
}
