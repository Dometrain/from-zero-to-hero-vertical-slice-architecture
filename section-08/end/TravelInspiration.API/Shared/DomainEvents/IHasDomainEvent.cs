using TravelInspiration.API.Shared.Domain;

namespace TravelInspiration.API.Shared.DomainEvents;

public interface IHasDomainEvent
{
    IList<DomainEvent> DomainEvents { get; }
}
