using Microsoft.Extensions.DependencyInjection;
using Moonpig.PostOffice.Data;

namespace Moonpig.PostOffice.Api
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceProvider)
        {
            serviceProvider.AddTransient<IDataProvider, DataProvider>();
            serviceProvider.AddTransient<IDespatchService, DespatchService>();
            serviceProvider.AddTransient<Data.Entities.DbContext>();

            return serviceProvider;
        }
    }
}