using SimpleChat.DAL.Entities;

namespace SimpleChat.BL.Helpers;

public static class MemberFactory
{
    public static Member CreateAdmin(Guid userId, Guid conversationId) =>
        new Member
        {
            UserId = userId,
            ConversationId = conversationId,
            UserRole = MemberRole.Admin
        };

    public static Member CreateMember(Guid userId, Guid conversationId) =>
        new Member
        {
            UserId = userId,
            ConversationId = conversationId,
        };
}