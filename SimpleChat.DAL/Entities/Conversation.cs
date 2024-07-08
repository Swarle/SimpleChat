using SimpleChat.DAL.Abstract;

namespace SimpleChat.DAL.Entities;

public class Conversation : Entity
{
    public required string Title { get; set; }
    public required string Tag { get; set; }
    public DateTime CreatedAd { get; set; } = DateTime.Now;
    public ICollection<Member> Members { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
}