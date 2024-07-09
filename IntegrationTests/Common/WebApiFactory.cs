using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimpleChat.API;
using SimpleChat.DAL.Context;
using Utility.Constants;

namespace IntegrationTests.Common;

public class WebApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationContext>));

            var connString = GetConnectionString();
            services.AddDbContext<DbContext, ApplicationContext>(opt => 
                opt.UseSqlServer(connString));
            
            RecreateDatabase(services);
        });
    }

    private static ApplicationContext GetDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

        return dbContext;
    }

    private static string GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("testSettings.json")
            .Build();

        var connString = configuration.GetConnectionString(SD.DefaultConnection) ??
                         throw new NullReferenceException("Unavailable to get connection string");
        return connString;
    }

    private static void RecreateDatabase(IServiceCollection services)
    {
        var dbContext = GetDbContext(services);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }
}