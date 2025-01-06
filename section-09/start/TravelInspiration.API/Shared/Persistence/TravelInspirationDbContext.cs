﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.DomainEvents;
using TravelInspiration.API.Shared.Security;

namespace TravelInspiration.API.Shared.Persistence;

public sealed class TravelInspirationDbContext(
    DbContextOptions<TravelInspirationDbContext> options, 
    IPublisher publisher,
    ICurrentUserService currentUserService) : DbContext(options)
{
    private readonly IPublisher _publisher = publisher;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public DbSet<Itinerary> Itineraries => Set<Itinerary>();
    public DbSet<Stop> Stops => Set<Stop>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
	 modelBuilder.Entity<Itinerary>().HasData(
		 new Itinerary("A Trip to Paris", "dummyuserid")
		 {
			 Id = 1,
			 Description = "Five great days in Paris",
			 CreatedBy = "DATASEED",
			 CreatedOn = new DateTime(2025, 01, 06)
		 },
		  new Itinerary("Antwerp Extravaganza", "dummyuserid")
		  {
			  Id = 2,
			  Description = "A week in beautiful Antwerp",
			  CreatedBy = "DATASEED",
			  CreatedOn = new DateTime(2025, 01, 06)
		  });

	 modelBuilder.Entity<Stop>().HasData(
		  new("The Eiffel Tower")
		  {
			  Id = 1,
			  ItineraryId = 1,
			  ImageUri = new Uri("https://localhost:7120/images/eiffeltower.jpg"),
			  CreatedBy = "DATASEED",
			  CreatedOn = new DateTime(2025, 01, 06)
		  },
		  new("The Louvre")
		  {
			  Id = 2,
			  ItineraryId = 1,
			  ImageUri = new Uri("https://localhost:7120/images/louvre.jpg"),
			  CreatedBy = "DATASEED",
			  CreatedOn = new DateTime(2025, 01, 06)
		  },
		  new("Père Lachaise Cemetery")
		  {
			  Id = 3,
			  ItineraryId = 1,
			  ImageUri = new Uri("https://localhost:7120/images/perelachaise.jpg"),
			  CreatedBy = "DATASEED",
			  CreatedOn = new DateTime(2025, 01, 06)
		  },
		  new("The Royal Museum of Beautiful Arts")
		  {
			  Id = 4,
			  ItineraryId = 2,
			  ImageUri = new Uri("https://localhost:7120/images/royalmuseum.jpg"),
			  CreatedBy = "DATASEED",
			  CreatedOn = new DateTime(2025, 01, 06)
		  },
		  new("Saint Paul's Church")
		  {
			  Id = 5,
			  ItineraryId = 2,
			  ImageUri = new Uri("https://localhost:7120/images/stpauls.jpg"),
			  CreatedBy = "DATASEED",
			  CreatedOn = new DateTime(2025, 01, 06)
		  },
		  new("Michelin Restaurant Visit")
		  {
			  Id = 6,
			  ItineraryId = 2,
			  ImageUri = new Uri("https://localhost:7120/images/michelin.jpg"),
			  CreatedBy = "DATASEED",
			  CreatedOn = new DateTime(2025, 01, 06)
		  });
	 modelBuilder.ApplyConfigurationsFromAssembly(
		 typeof(TravelInspirationDbContext).Assembly);
	 base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.LastModifiedOn = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    break;
            }
        }

        var domainEvents = ChangeTracker.Entries<IHasDomainEvent>()
            .Select(e => e.Entity.DomainEvents)
            .SelectMany(e => e)
            .Where(domainEvent => !domainEvent.IsPublished)
            .ToArray();

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
            domainEvent.IsPublished = true;
        }

        return await base.SaveChangesAsync(cancellationToken); 
    }
}
