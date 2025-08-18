using Domain.SharedKernel;
using Domain.SharedKernel.BaseEntity;
using Domain.SharedKernel.HelperFunctions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private ServiceProvider _serviceProvider;

        //  private AuthenticationTokenResponse authenticationTokenResponse;
        [TestInitialize] // run before each test
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddDomainShardKernelCollection(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build());
            _serviceProvider = services.BuildServiceProvider();
            // authenticationTokenResponse = _serviceProvider.GetService<AuthenticationTokenResponse>();

        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_serviceProvider != null)
            {
                _serviceProvider.Dispose();
            }
        }


    }
}