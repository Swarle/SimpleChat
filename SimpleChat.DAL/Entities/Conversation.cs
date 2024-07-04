using SimpleChat.DAL.Abstract;

namespace SimpleChat.DAL.Entities;

public class Conversation : Entity
{
    public required string Title { get; set; }
    public required DateTime CreatedAd { get; set; }
    public ICollection<UserToConversation> Members { get; set; } = [];
}