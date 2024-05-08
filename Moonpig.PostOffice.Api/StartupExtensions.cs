using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Moonpig.PostOffice.Api.Interfaces;
using Moonpig.PostOffice.Api.Services;
using Moonpig.PostOffice.Data;
using Moonpig.PostOffice.Data.Interfaces;

namespace Moonpig.PostOffice.Api
{
    [ExcludeFromCodeCoverage]
    public static class StartupExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDataProvider, DataProvider>();
            serviceCollection.AddTransient<IDespatchService, DespatchService>();
            serviceCollection.AddTransient<IDbContext, DbContext>();

            return serviceCollection;
        }
    }
}