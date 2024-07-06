using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleChat.BL.Helpers;
using SimpleChat.BL.Interfaces;
using SimpleChat.BL.Services;
using SimpleChat.DAL.Extensions;

namespace SimpleChat.BL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccessLayerServices(configuration);

        services.AddAutoMapper(typeof(MapperProfile).Assembly);
        
        services.AddScoped<IConversationService, ConversationService>();
        services.AddScoped<IConnectionService, ConnectionService>();
        services.AddScoped<IMessageService, MessageService>();

        return services;
    }
}