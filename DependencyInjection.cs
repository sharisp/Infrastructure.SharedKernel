
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
      
            

            services.TryAddScoped<ICurrentUser, CurrentUser>();

            services.TryAddScoped<IUnitOfWork, UnitOfWork>();
            services.TryAddSingleton(new AppHelper(configuration));

            return services;
        }
    }
}
