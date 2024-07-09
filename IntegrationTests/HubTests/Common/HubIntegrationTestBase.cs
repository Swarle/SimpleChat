using IntegrationTests.Common;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using SimpleChat.DAL.Entities;
using Utility.Constants;

namespace IntegrationTests.HubTests.Common;

public class HubIntegrationTestBase : IntegrationTestBase
{
    private readonly List<HubConnection> _connections = [];

    public override async Task TearDown()
    {
        foreach (var connection in _connections)
        {
            await connection.DisposeAsync();
        }
        
        await base.TearDown();
    }

    protected async Task<HubConnection> GetHubConnectionAsync(Guid? userId = null)
    {
        const string url = $"http://localhost/hubs/chat";

        var hubConnection = new HubConnectionBuilder()
            .WithUrl(url, options =>
            {
                options.HttpMessageHandlerFactory = _ => _testServer.CreateHandler();
                options.Headers.Add(SD.UserIdHeaderKey,
                    userId is not null ? userId.ToString()! : Guid.NewGuid().ToString());
            })
            .Build();

        await hubConnection.StartAsync();
        
        _connections.Add(hubConnection);

        return hubConnection;
    }
    
    protected async Task<List<User>> GetUserList()
    {
        var dbContext = GetDbContext();
        var user = await dbContext.Users.ToListAsync();

        return user;
    }
}