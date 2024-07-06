using SimpleChat.BL.DTO;

namespace SimpleChat.BL.Interfaces;

public interface IMessageService
{
    public Task<MessageDto> CreateMessage(CreateMessageDto createMessageDto);
}