using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SimpleChat.DAL.Context;

namespace IntegrationTests.Common;

public abstract class IntegrationTestBase
{
    private WebApiFactory _webApiFactory;
    private IServiceProvider _serviceProvider => _webApiFactory.Services;
    protected TestServer _testServer => _webApiFactory.Server;
    protected HttpClient _client;


    [SetUp]
    public async Task Setup()
    {
        _webApiFactory = new WebApiFactory();
        _client = _webApiFactory.CreateClient();
    }

    [TearDown]
    public virtual async Task TearDown()
    {
        _client.Dispose();
        await _webApiFactory.DisposeAsync();
    }
    
    protected ApplicationContext GetDbContext()
    {
        var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        return dbContext;
    }
}