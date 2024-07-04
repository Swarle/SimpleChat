using System.ComponentModel.DataAnnotations.Schema;
using SimpleChat.DAL.Abstract;

namespace SimpleChat.DAL.Entities;

public class UserToConversation : Entity
{
    public Guid UserId { get; set; }
    public Guid ConversationId { get; set; }
    public ConversationRole UserRole { get; set; } = ConversationRole.Member;

    public required User User { get; set; }
    public required Conversation Conversation { get; set; }
}

public enum ConversationRole
{
    Member,
    Admin
}