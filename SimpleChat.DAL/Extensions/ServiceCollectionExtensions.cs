using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleChat.DAL.Context;
using SimpleChat.DAL.Repository;
using Utility.Constants;

namespace SimpleChat.DAL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccessLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DbContext, ApplicationContext>(opt => 
            opt.UseSqlServer(configuration.GetConnectionString(SD.DefaultConnection)));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        return services;
    }
}