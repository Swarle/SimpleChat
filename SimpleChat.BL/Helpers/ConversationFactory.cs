using SimpleChat.DAL.Entities;

namespace SimpleChat.BL.Helpers;

public static class ConversationFactory
{
    public static Conversation CreateConversation(string title) =>
        new Conversation
        {
            Id = Guid.NewGuid(),
            Title = title
        };
}