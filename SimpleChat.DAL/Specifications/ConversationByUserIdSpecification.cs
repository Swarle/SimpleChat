using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Entities;

namespace SimpleChat.DAL.Specifications;

public class ConversationByUserIdSpecification : BaseSpecification<Conversation>
{
    public ConversationByUserIdSpecification(Guid userId)
        : base(c => c.Members.Any(m => m.UserId == userId))
    {
        AddInclude(nameof(Conversation.Members));
    }
}