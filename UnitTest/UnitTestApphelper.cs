using Domain.SharedKernel;
using Domain.SharedKernel.BaseEntity;
using Domain.SharedKernel.HelperFunctions;
using Infrastructure.SharedKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace UnitTest
{
    [TestClass]
    public class UnitTestApphelper
    {
        private ServiceProvider _serviceProvider;

        //  private AuthenticationTokenResponse authenticationTokenResponse;
        [TestInitialize] // run before each test
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddInfrastructureKernelCollection(new ConfigurationBuilder()
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

        [TestMethod]
        public void TestReadString()
        {

            var str = AppHelper.ReadAppSetting("AllowedHosts");
            Assert.IsNotNull(str);
            Assert.IsTrue(str.Length > 0);
            Assert.IsTrue(str == "*");
        }
        [TestMethod]
        public void TestReadSessionString()
        {

            var str = AppHelper.ReadAppSetting("Snowflake:WorkerId");
            Assert.IsNotNull(str);
            Assert.IsTrue(str.Length > 0);
            Assert.IsTrue(str == "1");
        }
        [TestMethod]
        public void TestNotExists()
        {

            var str = AppHelper.ReadAppSetting("Snowflake:WorkerId111");
            Assert.IsNull(str);
           
        }
    }
}