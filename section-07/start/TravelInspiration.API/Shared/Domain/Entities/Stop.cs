using TravelInspiration.API.Features.Stops;

namespace TravelInspiration.API.Shared.Domain.Entities;

public sealed class Stop(string name) : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = name;
    public Uri? ImageUri { get; set; }
    public int ItineraryId { get; set; }
    public Itinerary? Itinerary { get; set; }

    public void HandleCreateCommand(CreateStop.CreateStopCommand createStopCommand)
    {
        ImageUri = createStopCommand.ImageUri == null ? 
            null : new Uri(createStopCommand.ImageUri);
        ItineraryId = createStopCommand.ItineraryId;
    }
}
