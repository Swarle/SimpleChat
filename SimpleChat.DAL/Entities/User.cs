using SimpleChat.DAL.Abstract;

namespace SimpleChat.DAL.Entities;

public class User : Entity
{
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public ICollection<Member> Conversations { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
    public Connection? Connection { get; set; }
}