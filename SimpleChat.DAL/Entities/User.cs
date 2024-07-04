using SimpleChat.DAL.Abstract;

namespace SimpleChat.DAL.Entities;

public class User : Entity
{
    public required string Name { get; set; }
    public required DateTime CreatedAt { get; set; }
    public ICollection<UserToConversation> Conversations { get; set; } = [];
}