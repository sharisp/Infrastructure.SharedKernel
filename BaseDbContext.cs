using Domain.SharedKernel.BaseEntity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.SharedKernel
{
    public partial class BaseDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public BaseDbContext(DbContextOptions options)
            : base(options)
        {
        }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           // modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseDbContext).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseAuditableEntity).IsAssignableFrom(entityType.ClrType) &&
                    entityType.FindProperty(nameof(BaseAuditableEntity.IsDel)) != null)
                {
                    var method = typeof(BaseDbContext)
                        .GetMethod(nameof(ApplySoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)!
                        .MakeGenericMethod(entityType.ClrType);

                    method.Invoke(null, new object[] { modelBuilder });
                }
            }

        }
        private static void ApplySoftDeleteFilter<TEntity>(ModelBuilder modelBuilder)
            where TEntity : BaseAuditableEntity
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(e => !e.IsDel);
        }


    }
}
