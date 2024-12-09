using TravelInspiration.API.Shared.Domain.Entities;

namespace TravelInspiration.API.Shared.Domain.Events;

public class StopCreatedEvent(Stop stop) : DomainEvent
{
    public Stop Stop { get; } = stop;
}
