using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleChat.DAL.Context;
using Utility.Constants;

namespace SimpleChat.DAL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccessLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(opt => 
            opt.UseSqlServer(configuration.GetConnectionString(SD.DefaultConnection)));
        
        return services;
    }
}