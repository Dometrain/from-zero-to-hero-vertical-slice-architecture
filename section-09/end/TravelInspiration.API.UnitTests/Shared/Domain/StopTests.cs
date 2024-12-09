using TravelInspiration.API.Features.Stops;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Domain.Events;

namespace TravelInspiration.API.UnitTests.Shared.Domain;

public sealed class StopTests : IDisposable
{
    private readonly Stop _stopEntity;
    private readonly CreateStop.CreateStopCommand _createStopCommand;

    public StopTests()
    {
        _stopEntity = new Stop("StopForTesting");
        _createStopCommand = new CreateStop.CreateStopCommand(42,
            "A name",
            null);
    }

    [Fact]
    public void WhenExecutingHandleCreateCommand_WithItineraryId_StopItineraryIdMustMatch()
    {
        // ARRANGE
        // nothing to see here

        // ACT 
        _stopEntity.HandleCreateCommand(_createStopCommand);

        // ASSERT
        Assert.Equal(_createStopCommand.ItineraryId, 
            _stopEntity.ItineraryId);
    }

    [Fact]
    public void WhenExecutingCreateCommand_WithValidInput_OneStopCreatedEventMustBeAdded()
    {
        // ARRANGE
        // nothing to see here

        // ACT 
        _stopEntity.HandleCreateCommand(_createStopCommand);

        // ASSERT
        Assert.Single(_stopEntity.DomainEvents);
        Assert.IsType<StopCreatedEvent>(_stopEntity.DomainEvents[0]);
    }

    public void Dispose()
    {
        // no code here
    }
}
