using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Persistence;

namespace TravelInspiration.API.Features.Itineraries;

public static class GetItineraries
{
    public class GetItinerariesQuery
    { }

    public class GetItinerariesHandler
    { }

    public class GetItinerariesResponse
    { }


    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "api/itineraries",
            async (string? searchFor, 
                ILoggerFactory logger, 
                TravelInspirationDbContext dbContext,
                IMapper mapper,
                CancellationToken cancellationToken) =>
            {
                logger.CreateLogger("EndpointHandlers")
                    .LogInformation("GetItineraries feature called.");

                var result = await dbContext.Itineraries.Where(i =>
                    searchFor == null || 
                    i.Name.Contains(searchFor) ||
                    (i.Description != null && i.Description.Contains(searchFor)))
                    .ToListAsync(cancellationToken);

                return Results.Ok(mapper.Map<IEnumerable<ItineraryDto>>(await dbContext.Itineraries.Where(i =>
                     searchFor == null ||
                     i.Name.Contains(searchFor) ||
                     (i.Description != null && i.Description.Contains(searchFor)))
                     .ToListAsync(cancellationToken)));
            });
    }

    public sealed class ItineraryDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string UserId { get; set; }
    }

    public sealed class ItineraryMapProfile : Profile
    {
        public ItineraryMapProfile()
        {
            CreateMap<Itinerary, ItineraryDto>();
        }
    }
}
