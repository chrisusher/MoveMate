using dotenv.net;

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
        }
    }
}