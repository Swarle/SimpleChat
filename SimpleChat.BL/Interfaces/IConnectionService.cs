namespace SimpleChat.BL.Interfaces;

public interface IConnectionService
{
    public Task CreateConnection(string connectionId);
    public Task<List<string>> GetConnectionsByConversationId(Guid conversationId);
    public Task DeleteConnection();
}