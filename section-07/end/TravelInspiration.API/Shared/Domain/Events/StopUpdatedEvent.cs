using TravelInspiration.API.Shared.Domain.Entities;

namespace TravelInspiration.API.Shared.Domain.Events;

public sealed class StopUpdatedEvent(Stop stop) : DomainEvent
{
    public Stop Stop { get; } = stop;
}
