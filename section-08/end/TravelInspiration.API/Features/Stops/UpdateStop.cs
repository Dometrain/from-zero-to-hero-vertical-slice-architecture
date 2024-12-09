using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Domain.Events;
using TravelInspiration.API.Shared.Persistence;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Stops;

public sealed class UpdateStop : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPut(
            "api/itineraries/{itineraryId}/stops/{stopId}",
            (int itineraryId,
            int stopId,
            UpdateStopCommand updateStopCommand,
            IMediator mediator,
            CancellationToken cancellationToken) =>
            {
                updateStopCommand.ItineraryId = itineraryId;
                updateStopCommand.StopId = stopId;
                return mediator.Send(updateStopCommand, cancellationToken);
            }).RequireAuthorization(Shared.Security.AuthorizationPolicies.HasWriteActionPolicy);
    }

    public sealed class UpdateStopCommand : IRequest<IResult>
    {
        public int ItineraryId { get; set; }
        public int StopId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUri { get; set; }
        public bool? Suggested { get; set; }
    }

    public sealed class UpdateStopCommandValidator : AbstractValidator<UpdateStopCommand>
    {
        public UpdateStopCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
            RuleFor(v => v.ImageUri)
                .Must(ImageUri => Uri.TryCreate(ImageUri ?? "", UriKind.Absolute, out _))
                .When(v => !string.IsNullOrEmpty(v.ImageUri))
                .WithMessage("ImageUri must be a valid Uri.");
        }
    }

    public sealed class UpdateStopCommandHandler(
        TravelInspirationDbContext dbContext,
        IMapper mapper) : IRequestHandler<UpdateStopCommand, IResult>
    {
        private readonly TravelInspirationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<IResult> Handle(UpdateStopCommand request, 
            CancellationToken cancellationToken)
        {
            var stopToUpdate = await _dbContext.Stops
                .FirstOrDefaultAsync(s => s.Id == request.StopId
                    && s.ItineraryId == request.ItineraryId,
                    cancellationToken);

            if (stopToUpdate == null)
            {
                return Results.NotFound();
            }

            stopToUpdate.HandleUpdateCommand(request);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Results.Ok(_mapper.Map<StopDto>(stopToUpdate));            
        }
    }

    public sealed class StopDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public bool? Suggested { get; set; }
        public Uri? ImageUri { get; set; }
        public required int ItineraryId { get; set; }
    }

    public sealed class StopMapProfileAfterUpdate : Profile
    {
        public StopMapProfileAfterUpdate()
        {
            CreateMap<Stop, StopDto>();
        }
    }

    public sealed class SuggestStopStopUpdatedEventHandler(
        ILogger<SuggestStopStopUpdatedEventHandler> logger)
        : INotificationHandler<StopUpdatedEvent>
    {
        private readonly ILogger<SuggestStopStopUpdatedEventHandler> _logger = logger;

        public Task Handle(StopUpdatedEvent notification, 
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Listener {listener} to domain event {domainEvent} triggered.",
                GetType().Name,
                notification.GetType().Name);

            // do some AI magic to change (a) generated stop(s) for an itinerary
            return Task.CompletedTask;
        }
    }
}
