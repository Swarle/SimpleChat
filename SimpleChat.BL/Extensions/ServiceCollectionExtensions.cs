using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleChat.DAL.Extensions;

namespace SimpleChat.BL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLayerServices(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddDataAccessLayerServices(configuration);

        return service;
    }
}