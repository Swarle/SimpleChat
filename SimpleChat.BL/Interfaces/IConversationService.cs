using SimpleChat.BL.DTO;

namespace SimpleChat.BL.Interfaces;

public interface IConversationService
{
    public Task<string> CreateConversationAsync(CreateConversationDto conversationDto);
    public Task<string> JoinToConversationAsync(Guid conversationId);
    public Task<string> DeleteConversationAsync(Guid conversationId);
    public Task<List<string>> GetAllUserConversationsAsync();
}