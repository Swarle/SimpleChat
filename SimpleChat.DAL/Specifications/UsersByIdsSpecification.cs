using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Entities;

namespace SimpleChat.DAL.Specifications;

public class UsersByIdsSpecification : BaseSpecification<User>
{
    public UsersByIdsSpecification(List<Guid> userIds) 
        : base(u => userIds.Any(id => u.Id == id))
    {
        
    }
}