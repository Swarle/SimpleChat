using Microsoft.OpenApi.Models;

namespace SimpleChat.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerGenConfigured(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SimpleChat",
                Version = "v1"
            });
        });
        
        return services;
    }
}