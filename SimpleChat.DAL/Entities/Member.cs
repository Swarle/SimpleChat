using System.ComponentModel.DataAnnotations.Schema;
using SimpleChat.DAL.Abstract;

namespace SimpleChat.DAL.Entities;

public class Member : Entity
{
    public Guid UserId { get; set; }
    public Guid ConversationId { get; set; }
    public MemberRole UserRole { get; set; } = MemberRole.Member;

    public User User { get; set; } = null!;
    public Conversation Conversation { get; set; } = null!;
}

public enum MemberRole
{
    Member,
    Admin
}