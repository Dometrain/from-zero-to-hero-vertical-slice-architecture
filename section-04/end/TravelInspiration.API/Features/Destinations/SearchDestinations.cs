using TravelInspiration.API.Shared.Networking;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Destinations;

public sealed class SearchDestinations : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("api/destinations",
            async (string? searchFor,
                ILoggerFactory logger,
                CancellationToken cancellationToken,
                IDestinationSearchApiClient destinationSearchApiClient) =>
        {
            logger.CreateLogger("EndpointHandlers")
                .LogInformation("SearchDestinations feature called.");

            var resultFromApiCall = await destinationSearchApiClient
                .GetDestinationsAsync(searchFor, cancellationToken); 

            // project the result
            var result = resultFromApiCall.Select(d => new
            {                
                d.Name,
                d.Description,
                d.ImageUri                
            });

            return Results.Ok(result);
        });
    }
}
