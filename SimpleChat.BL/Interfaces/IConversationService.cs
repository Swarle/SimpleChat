using SimpleChat.BL.DTO;

namespace SimpleChat.BL.Interfaces;

public interface IConversationService
{
    public Task<string> CreateConversationAsync(CreateConversationDto conversationDto);
    public Task<string> JoinToConversationAsync(Guid conversationId);
    public Task<string> DeleteConversationAsync(Guid conversationId);
    public Task<IEnumerable<ConversationSearchDto>> GetConversationByTitleOrTagAsync(string query, CancellationToken cancellationToken = default);
    public Task<ConversationDto> GetConversationByIdAsync(Guid conversationId, CancellationToken cancellationToken = default);
    public Task UpdateConversationAsync(UpdateConversationDto updateConversationDto, CancellationToken cancellationToken = default);
    public Task<List<string>> GetAllUserConversationsAsync();
}