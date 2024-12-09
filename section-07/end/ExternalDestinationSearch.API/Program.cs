using ExternalDestinationSearch.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
 
List<DestinationDto> destinations = [
    new DestinationDto("Antwerp, Belgium") { Description = "", ImageName = "antwerp.jpg" },
    new DestinationDto("San Francisco, USA") { Description = "", ImageName = "sanfranciso.jpg" },
    new DestinationDto("Sydney, Australia") { Description = "", ImageName = "sydney.jpg" },
    new DestinationDto("Paris, France") { Description = "", ImageName = "paris.jpg" },
    new DestinationDto("New Delhi, India") { Description = "", ImageName = "newdelhi.jpg" },
    new DestinationDto("Tokyo, Japan") { Description = "", ImageName = "tokyo.jpg" },
    new DestinationDto("Cape Town, South Africa") { Description = "", ImageName = "capetown.jpg" },
    new DestinationDto("Barcelona, Spain") { Description = "", ImageName = "barcelona.jpg" },
    new DestinationDto("Toronto, Canada") { Description = "", ImageName = "toronto.jpg" }
];

app.MapGet("/destinations", (string? searchFor, HttpContext context) =>
{ 
    var filteredDestinations = destinations.Where(d => 
        searchFor == null ||
        d.Name.Contains(searchFor) || 
        (d.Description != null && d.Description.Contains(searchFor)));

    foreach (var destination in filteredDestinations)
    {
        // fill our correct ImageRootUri
        destination.ImageRootUri = $"{context.Request.Scheme}://{context.Request.Host}/images/";
    }
    return Results.Ok(filteredDestinations);
});

app.Run();