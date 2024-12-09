using System.Collections;
using TravelInspiration.API.Shared.Domain.Models;

namespace TravelInspiration.API.Shared.Networking
{
    public interface IDestinationSearchApiClient
    {
        Task<IEnumerable<Destination>> GetDestinationsAsync(string? searchFor, CancellationToken? cancellationToken);
    }
}