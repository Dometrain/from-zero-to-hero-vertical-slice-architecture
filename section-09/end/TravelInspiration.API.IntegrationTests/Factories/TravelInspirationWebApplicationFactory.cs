using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Metadata.Ecma335;
using TravelInspiration.API.Shared.Security;

namespace TravelInspiration.API.IntegrationTests.Factories;

public sealed class TravelInspirationWebApplicationFactory 
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "ConnectionStrings:TravelInspirationDbConnection",
                      "Server=(localdb)\\mssqllocaldb;Database=TravelInspirationDb_IntegrationTests;Trusted_Connection=True;MultipleActiveResultSets=true;" }
                });
            
        });

        // register a dummy user service for use during tests
        // that overrides the real implementation
        builder.ConfigureServices((services) =>
        {
            services.AddScoped<ICurrentUserService, DummyUserService>();   
        });

        base.ConfigureWebHost(builder);
    }

    public sealed class DummyUserService : ICurrentUserService
    {
        public string? UserId { get { return "TESTUSER"; } }
    }
}


