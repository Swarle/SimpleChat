using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleChat.DAL.Context;
using SimpleChat.DAL.Helpers;

namespace SimpleChat.BL.Extensions;

public static class HostExtensions
{
    public static async Task SeedDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationContext>();

        await Seed.SeedDatabaseAsync(context);
    }

    public static IHostApplicationLifetime  RegisterCleanupConnections(this IHostApplicationLifetime  hostLifetime, IHost host)
    {
        hostLifetime.ApplicationStopping.Register(async () =>
        {
            using var scope = host.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            await CleanupConnections.CleanupConnectionsAsync(context);
        });
        
        return hostLifetime;
    }
}