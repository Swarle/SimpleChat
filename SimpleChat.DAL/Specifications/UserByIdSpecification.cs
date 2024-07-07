using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Entities;

namespace SimpleChat.DAL.Specifications;

public class UserByIdSpecification : BaseSpecification<User>
{
    public UserByIdSpecification(List<Guid> userIds) 
        : base(u => userIds.Any(id => u.Id == id))
    {
        
    }

    public UserByIdSpecification(Guid userId, Guid conversationId)
        : base(u => u.Id == userId && u.Conversations.Any(c => c.ConversationId == conversationId))
    {
        AddInclude($"{nameof(User.Conversations)}");
        AddInclude($"{nameof(User.Conversations)}.{nameof(Member.Conversation)}");
    }
}