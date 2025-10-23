using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Common.Application;
using Common.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Trading.Domain.AggregatesModel.OrderAggregate;

namespace Trading.Infrastructure
{
    public class TradingDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderFill> OrderFills { get; set; } = null!;

        public TradingDbContext(DbContextOptions<TradingDbContext> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TradingDbContext).Assembly);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEventsAsync(this);
            await base.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext context)
        {
            var domainEntities = context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents.Any())
                .Select(x => x.Entity);

            var domainEvents = domainEntities
                .SelectMany(x => x.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}
