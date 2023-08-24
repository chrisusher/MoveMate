using dotenv.net;

namespace Services.Tests
{
    [SetUpFixture]
    public class AssemblyLifecycle
    {
        [OneTimeSetUp]
        public static void AssemblySetup()
        {
            DotEnv.Load();
        }
    }
}