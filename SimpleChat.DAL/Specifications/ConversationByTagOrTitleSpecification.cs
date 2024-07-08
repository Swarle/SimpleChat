using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Entities;

namespace SimpleChat.DAL.Specifications;

public class ConversationByTagOrTitleSpecification : BaseSpecification<Conversation>
{
    public ConversationByTagOrTitleSpecification(string query)
    {
        query = query.ToLowerInvariant();
        
        AddExpression(c => c.Title.ToLower().Contains(query) ||
                           c.Tag.ToLower().Contains(query));
    }
}