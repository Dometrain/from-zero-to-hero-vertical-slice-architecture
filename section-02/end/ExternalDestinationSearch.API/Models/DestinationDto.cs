namespace ExternalDestinationSearch.API.Models;

public class DestinationDto(string name)
{
    public string Name { get; set; } = name;
    public string? Description { get; set; }
    public string? ImageRootUri { get; set; }
    public string? ImageName { get; set; }
    public Uri? ImageUri
    {
        get {
            return (ImageName == null || ImageRootUri == null) ? 
                null : new Uri(ImageRootUri + ImageName);
        }
    }
}
