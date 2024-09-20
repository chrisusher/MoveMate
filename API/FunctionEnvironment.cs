namespace ChrisUsher.MoveMate.API;

public static class FunctionEnvironment
{
    public static string EnvironmentName => Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");

    public static bool IsDevelopment => EnvironmentName.ToLower() == "development";

    public static string RootUrl
    {
        get
        {
            var scheme = "https";

            if (IsDevelopment)
            {
                scheme = "http";
            }

            return $"{scheme}://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}";
        }
    }
}