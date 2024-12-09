using TravelInspiration.API.Features.Stops;
using TravelInspiration.API.Shared.Domain.Events;
using TravelInspiration.API.Shared.DomainEvents;

namespace TravelInspiration.API.Shared.Domain.Entities;

public sealed class Stop(string name) : AuditableEntity, IHasDomainEvent
{
    public int Id { get; set; }
    public string Name { get; set; } = name;
    public Uri? ImageUri { get; set; }
    public int ItineraryId { get; set; }
    public bool? Suggested { get; set; }
    public Itinerary? Itinerary { get; set; }
    public IList<DomainEvent> DomainEvents { get; } = [];

    public void HandleCreateCommand(CreateStop.CreateStopCommand createStopCommand)
    {
        ImageUri = createStopCommand.ImageUri == null ? 
            null : new Uri(createStopCommand.ImageUri);
        ItineraryId = createStopCommand.ItineraryId;
        DomainEvents.Add(new StopCreatedEvent(this));
    }

    public void HandleUpdateCommand(UpdateStop.UpdateStopCommand updateStopCommand)
    {
        Name = updateStopCommand.Name;
        ImageUri = updateStopCommand.ImageUri == null ? 
            null : new Uri(updateStopCommand.ImageUri);
        Suggested = updateStopCommand.Suggested;
        DomainEvents.Add(new StopUpdatedEvent(this));
    }
}
