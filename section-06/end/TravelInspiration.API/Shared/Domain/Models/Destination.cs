namespace TravelInspiration.API.Shared.Domain.Models
{
    public class Destination(string name)
    {
        public required string Name { get; set; } = name;
        public string? Description { get; set; }
        public string? ImageRootUri { get; set; }
        public string? ImageName { get; set; }
        public Uri? ImageUri { get; set; } 
    }
}
