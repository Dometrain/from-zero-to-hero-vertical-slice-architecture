using TravelInspiration.API.Shared.Networking;

namespace TravelInspiration.API.Features.Destinations;

public static class SearchDestinations
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/destinations",
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
