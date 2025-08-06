using Domain.SharedKernel.BaseEntity;
using Domain.SharedKernel.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SharedKernel
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ICurrentUser currentUser;
        private readonly IMediator mediatR;
        private readonly BaseDbContext dbContext;
        public UnitOfWork(DbContextOptions options, ICurrentUser currentUser, IMediator mediatR, BaseDbContext dbContext)
        {
            this.currentUser = currentUser;
            this.mediatR = mediatR;
            this.dbContext = dbContext;
        }
        public int SaveChanges()
        {
            UpdateEntities(dbContext);
            return dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateEntities(dbContext);
            var res = await dbContext.SaveChangesAsync(cancellationToken);
            if (res > 0)
            {
                await DispatchEvents(dbContext);
            }
            return res;
        }
        /// <summary>
        /// update the entities with audit information like creator, updater, and timestamps.
        /// </summary>
        /// <param name="context"></param>
        public void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
            {
                if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    var now = DateTimeOffset.UtcNow;

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatorUserId = currentUser?.UserId;
                            entry.Entity.CreateDateTime = now;
                            entry.Entity.IsDel = false;
                            break;

                        case EntityState.Modified:
                            if (entry.HasChangedOwnedEntities() || entry.Properties.Any(p => p.IsModified))
                            {
                                entry.Entity.UpdaterUserId = currentUser?.UserId;
                                entry.Entity.UpdateDateTime = now;
                            }
                            break;

                            //using soft delete function instead 
                            /*  case EntityState.Deleted:
                                  entry.State = EntityState.Modified;
                                  entry.Entity.IsDel = true;
                                  entry.Entity.DeleterUserId = currentUser?.UserId;
                                  entry.Entity.DeleteDateTime = now;
                                  break;*/
                    }
                }
            }
        }
        /// <summary>
        /// publish domain events for entities that have them after saving changes to the database.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task DispatchEvents(DbContext? context)
        {
            if (context == null) return;

            var entities = context.ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.Entity.GetDomainEvents().Any())
                .Select(e => e.Entity);

            var domainEvents = entities
                .SelectMany(e => e.GetDomainEvents())
                .ToList();

            entities.ToList().ForEach(e => e.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediatR.Publish(domainEvent);

        }
    }
}
