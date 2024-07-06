using SimpleChat.DAL.Abstract;

namespace SimpleChat.DAL.Entities;

public class Connection : Entity
{
    public required string ConnectionId { get; set; }
    public User User { get; set; } = null!;
}