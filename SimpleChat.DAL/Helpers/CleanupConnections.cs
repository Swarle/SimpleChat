using SimpleChat.DAL.Context;

namespace SimpleChat.DAL.Helpers;

public static class CleanupConnections
{
    public static async Task CleanupConnectionsAsync(ApplicationContext context)
    {
        context.Connections.RemoveRange(context.Connections);
        await context.SaveChangesAsync();
    }
}