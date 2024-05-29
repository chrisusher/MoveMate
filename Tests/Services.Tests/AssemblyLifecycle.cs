using dotenv.net;
using Microsoft.Extensions.Configuration;
using DockerBuilder = Ductus.FluentDocker.Builders.Builder;

namespace Services.Tests
{
    [SetUpFixture]
    public class AssemblyLifecycle
    {

        [OneTimeSetUp]
        public static void AssemblySetup()
        {
#if DEBUG
            DotEnv.Load(new DotEnvOptions(probeForEnv: true, ignoreExceptions: false));
#endif
            var builder = new DockerBuilder()
                .UseContainer()
                .UseCompose()
                .FromFile("docker-compose.yml");

            ServiceTestsCommon.DockerServices = builder.Build().Start();

            ServiceTestsCommon.Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            ServiceTestsCommon.DockerServices?.Dispose();
        }
    }
}