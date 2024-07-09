using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SimpleChat.API.Extensions;
using SimpleChat.API.Filters;
using SimpleChat.API.Hubs;
using SimpleChat.API.Middlewares;
using SimpleChat.BL.Extensions;

namespace SimpleChat.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .AddNewtonsoftJson(opt =>
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        
        builder.Services.AddCors(options =>
            options.AddDefaultPolicy(policy => policy
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(origin => true)
            ));
        
        builder.Services.AddSignalR(opt =>
            opt.AddFilter<HubExceptionFilter>());
        
        builder.Services.AddAuthorization();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddBusinessLayerServices(builder.Configuration);
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGenConfigured();

        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();
        app.UseApiExceptionMiddleware();

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();
        app.MapHub<ConversationHub>("/hubs/chat");

        await app.SeedDatabaseAsync();

        app.Lifetime.RegisterCleanupConnections(app);
        
        await app.RunAsync();
    }
}