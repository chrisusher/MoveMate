using System.Diagnostics;
using dotenv.net;
using Microsoft.Extensions.Configuration;

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

            Process.Start(new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = "compose up -d --remove-orphans",
                UseShellExecute = false,
                WorkingDirectory = Environment.CurrentDirectory
            });

            ServiceTestsCommon.Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = "compose down",
                UseShellExecute = false,
                WorkingDirectory = Environment.CurrentDirectory
            });
        }
    }
}