using ChrisUsher.MoveMate.API.Services.StampDuty;

namespace Services.Tests
{
    public class ServiceTestsCommon
    {
        private static ServiceProvider _services;

        public static ServiceProvider Services 
        {
            get
            {
                if(_services == null)
                {
                    _services = RegisterServices();
                }
                return _services;
            }
        }

        private static ServiceProvider RegisterServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<StampDutyService>();

            return services.BuildServiceProvider();
        }        
    }
}