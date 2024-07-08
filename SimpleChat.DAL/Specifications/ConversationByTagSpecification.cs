using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Entities;

namespace SimpleChat.DAL.Specifications;

public class ConversationByTagSpecification : BaseSpecification<Conversation>
{
    public ConversationByTagSpecification(string tag)
        : base(c => c.Tag == tag)
    {
        
    }
}