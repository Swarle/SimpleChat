using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Entities;

namespace SimpleChat.DAL.Specifications;

public class ConversationWithMembersSpecification : BaseSpecification<Conversation>
{
    public ConversationWithMembersSpecification(Guid conversationId, Guid userId)
        : base(c => c.Id == conversationId && c.Members.Any(m => m.UserId == userId))
    {
        AddInclude($"{nameof(Conversation.Members)}");
    }
}