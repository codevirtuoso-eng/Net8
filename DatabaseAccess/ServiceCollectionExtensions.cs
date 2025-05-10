using DatabaseAccess.Data.Context;
using DatabaseAccess.Data.DataAccess;
using DatabaseAccess.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseAccess
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection RegisterDataServices(this IServiceCollection services, IConfiguration configuration, string connectionString)
        {
            // Register data function services
            services.AddTransient<IOrderData, OrderData>();
            services.AddTransient<IUserData, UserData>();
            services.AddTransient<IMenuListingData, MenuListingData>();

            services.AddDbContext<MainAppDbContext>(o =>
                o.UseSqlServer(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            return services;
        }
    }
}

