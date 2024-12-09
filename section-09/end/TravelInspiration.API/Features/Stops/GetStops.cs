using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Persistence;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Stops;

public sealed class GetStops : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet(
            "api/itineraries/{itineraryId}/stops",
            (int itineraryId, 
                IMediator mediator,
                CancellationToken cancellationToken) =>
            { 
                 return mediator.Send(new GetStopsQuery(itineraryId), 
                     cancellationToken); 
            }).RequireAuthorization();
    }

    public sealed class GetStopsQuery(int itineraryId) : IRequest<IResult>
    {
        public int ItineraryId { get; } = itineraryId;
    }

    public sealed class GetStopsHandler(TravelInspirationDbContext dbContext,
        IMapper mapper) : IRequestHandler<GetStopsQuery, IResult>
    {
        private readonly TravelInspirationDbContext _dbContext = dbContext;

        public async Task<IResult> Handle(GetStopsQuery request, 
            CancellationToken cancellationToken)
        {
            var itinerary = await _dbContext.Itineraries
                    .Include(i => i.Stops)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(i => i.Id == request.ItineraryId, 
                    cancellationToken);

            if (itinerary == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(mapper.Map<IEnumerable<StopDto>>(itinerary.Stops));
        }
    }

    public sealed class StopDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public bool? Suggested { get; set; }
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
