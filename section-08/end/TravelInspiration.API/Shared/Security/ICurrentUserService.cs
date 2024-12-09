namespace TravelInspiration.API.Shared.Security
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
    }
}