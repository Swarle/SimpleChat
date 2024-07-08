using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Entities;

namespace SimpleChat.DAL.Specifications;

public class ConversationByIdSpecification : BaseSpecification<Conversation>
{
    public ConversationByIdSpecification(Guid conversationId) :
        base(c => c.Id == conversationId)
    {
        AddInclude($"{nameof(Conversation.Messages)}");
        AddInclude($"{nameof(Conversation.Members)}");
        AddInclude($"{nameof(Conversation.Members)}.{nameof(Member.User)}");
    }
}