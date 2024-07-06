using SimpleChat.BL.DTO;
using SimpleChat.DAL.Entities;

namespace SimpleChat.BL.Helpers;

public static class MessageFactory
{
    public static Message CreateMessage(CreateMessageDto createMessageDto, Guid senderId) =>
        new Message
        {
            ReceiverConversationId = createMessageDto.RecipientConversationId,
            SenderId = senderId,
            Text = createMessageDto.Text
        };
}