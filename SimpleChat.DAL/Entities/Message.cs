using SimpleChat.DAL.Abstract;

namespace SimpleChat.DAL.Entities;

public class Message : Entity
{
    public Guid SenderId { get; set; }
    public User User { get; set; } = null!;
    public Guid ReceiverConversationId { get; set; }
    public Conversation Conversation { get; set; } = null!;
    
    public required string Text { get; set; }
    public DateTime SendAt { get; set; } = DateTime.Now;
}