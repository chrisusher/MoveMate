using dotenv.net;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.Extensions.Configuration;

namespace Services.Tests
{
    [SetUpFixture]
    public class AssemblyLifecycle
    {
        private static IContainerService _storageDocker;

        [OneTimeSetUp]
        public static void AssemblySetup()
        {
#if DEBUG
            DotEnv.Load(new DotEnvOptions(probeForEnv: true, ignoreExceptions: false));
#endif
            _storageDocker = new Builder()
                .UseContainer()
                .UseImage("mcr.microsoft.com/azure-storage/azurite:latest")
                .ExposePort(10000, 10000)
                .Build()
                .Start();
            _storageDocker.StopOnDispose = true;

            ServiceTestsCommon.Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            _storageDocker?.Dispose();
        }
    }
}