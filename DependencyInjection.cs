
using Domain.SharedKernel;
using Domain.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace Infrastructure.SharedKernel
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureKernelCollection(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddDomainShardKernelCollection(configuration);
            // 如果是多个DB，这儿可以改成 自定义的DBContext, 继承于DbContext即可
            services.AddDbContext<BaseDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            });
            

            services.TryAddScoped<ICurrentUser, CurrentUser>();

            services.TryAddScoped<IUnitOfWork, UnitOfWork>();
            services.TryAddSingleton(new AppHelper(configuration));

            return services;
        }
    }
}
