using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TravelInspiration.API.IntegrationTests.Factories;
using TravelInspiration.API.Shared.Persistence;

namespace TravelInspiration.API.IntegrationTests.Fixtures;

public sealed class SliceFixture
{
    private readonly TravelInspirationWebApplicationFactory _factory;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private IServiceScope? _scope;

    private static readonly object _lock = new();
    private static bool _databaseInitialized;


    public SliceFixture()
    {
        _factory = new TravelInspirationWebApplicationFactory();
        _serviceScopeFactory = _factory.Services
            .GetRequiredService<IServiceScopeFactory>();

        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    using (var context = CreateContext(scope))
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        context.Stops.RemoveRange(context.Stops.ToList());
                        context.Itineraries.RemoveRange(context.Itineraries.ToList());
                        context.SaveChanges();

                    }
                    _databaseInitialized = true;
                }
            }
        }
    }

    private TravelInspirationDbContext CreateContext(IServiceScope scope)
    {
        var context = scope.ServiceProvider
            .GetRequiredService<TravelInspirationDbContext>();
        return context;
    }

    public async Task ExecuteInTransactionAsync(
        Func<TravelInspirationDbContext, Task> actionToExecute)
    {
        using (_scope = _serviceScopeFactory.CreateScope())
        {
            var context = CreateContext(_scope);
            await context.Database.BeginTransactionAsync();
            await actionToExecute(context);
            await context.Database.RollbackTransactionAsync();
        }
    }

    public async Task SendAsync(IRequest<IResult> cmdOrQuery)
    {
        if (_scope == null)
        {
            throw new ArgumentException("SendAsync must be run in the context of a scope.");
        }
        var mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(cmdOrQuery);
    }
}
