using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Entities;

namespace SimpleChat.DAL.Specifications;

public class UserByIdSpecification : BaseSpecification<User>
{
    public UserByIdSpecification(List<Guid> userIds) 
        : base(u => userIds.Any(id => u.Id == id))
    {
        
    }
}