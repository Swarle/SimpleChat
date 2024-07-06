using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Entities;

namespace SimpleChat.DAL.Specifications;

public class ConnectionByConversationIdSpecification : BaseSpecification<Connection>
{
    public ConnectionByConversationIdSpecification(Guid conversationId)
    : base(c => c.User.Conversations.Any(e => e.ConversationId == conversationId))
    {
        AddInclude($"{nameof(Connection.User)}");
        AddInclude($"{nameof(User)}.{nameof(User.Conversations)}");
    }
}