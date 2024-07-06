using SimpleChat.BL.DTO;

namespace SimpleChat.BL.Interfaces;

public interface IConversationService
{
    public Task<string> CreateConversation(CreateConversationDto conversationDto);
    public Task<List<string>> GetAllUserConversationsAsync();
}