using SimpleChat.BL.DTO;

namespace SimpleChat.BL.Interfaces;

public interface IMessageService
{
    public Task<MessageDto> CreateMessageAsync(CreateMessageDto createMessageDto);
}