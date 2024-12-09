using System.ComponentModel.DataAnnotations;

namespace TravelInspiration.API.Shared.Domain.Entities;

public sealed class Itinerary(string name, string userId) : AuditableEntity
{
    public int Id { get; set; } 
    public string Name { get; set; } = name;
    public string? Description { get; set; }
    public string UserId { get; set; } = userId;
    public ICollection<Stop> Stops { get; set; } = [];
}
