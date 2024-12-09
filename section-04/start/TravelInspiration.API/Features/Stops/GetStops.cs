using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Persistence;

namespace TravelInspiration.API.Features.Stops;

public static class GetStops
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "api/itineraries/{itineraryId}/stops",
            async (int itineraryId,
                ILoggerFactory logger,
                TravelInspirationDbContext dbContext,
                IMapper mapper,
                CancellationToken cancellationToken) =>
            {
                logger.CreateLogger("EndpointHandlers")
                    .LogInformation("GetStops feature called.");

                var itinerary = await dbContext.Itineraries
                    .Include(i => i.Stops)
                    .FirstOrDefaultAsync(i => i.Id == itineraryId, cancellationToken);

                if (itinerary == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(mapper.Map<IEnumerable<StopDto>>(itinerary.Stops));
            });
    }

    public sealed class StopDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public Uri? ImageUri { get; set; }
        public required int ItineraryId { get; set; }
    }

    public sealed class StopMapProfile : Profile
    {
        public StopMapProfile()
        {
            CreateMap<Stop, StopDto>();    
        }
    }
}
