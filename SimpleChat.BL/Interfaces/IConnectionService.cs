namespace SimpleChat.BL.Interfaces;

public interface IConnectionService
{
    public Task CreateConnectionAsync(string connectionId);
    public Task<List<string>> GetConnectionsByConversationIdAsync(Guid conversationId);
    public Task DeleteConnectionAsync();
}