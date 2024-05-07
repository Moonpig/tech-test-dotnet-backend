using Microsoft.Extensions.DependencyInjection;
using Moonpig.PostOffice.Api.Interfaces;
using Moonpig.PostOffice.Api.Services;
using Moonpig.PostOffice.Data;
using Moonpig.PostOffice.Data.Interfaces;

namespace Moonpig.PostOffice.Api
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceProvider)
        {
            serviceProvider.AddTransient<IDataProvider, DataProvider>();
            serviceProvider.AddTransient<IDespatchService, DespatchService>();
            serviceProvider.AddTransient<DbContext>();

            return serviceProvider;
        }
    }
}