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
            try
            {
                DotEnv.Load(new DotEnvOptions(probeForEnv: true, ignoreExceptions: false));
            }
            catch (Exception)
            {
                if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") != "true")
                {
                    throw;
                }
            }

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