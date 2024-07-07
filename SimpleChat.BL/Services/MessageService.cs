using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SimpleChat.BL.DTO;
using SimpleChat.BL.Extensions;
using SimpleChat.BL.Helpers;
using SimpleChat.BL.Interfaces;
using SimpleChat.DAL.Entities;
using SimpleChat.DAL.Repository;

namespace SimpleChat.BL.Services;

public class MessageService : IMessageService
{
    private readonly IRepository<Message> _messageRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Conversation> _conversationRepository;
    private readonly IMapper _mapper;
    private readonly HttpContext _context;

    public MessageService(IRepository<Message> messageRepository,
        IHttpContextAccessor accessor,
        IRepository<User> userRepository,
        IRepository<Conversation> conversationRepository,
        IMapper mapper)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _conversationRepository = conversationRepository;
        _mapper = mapper;
        _context = accessor.HttpContext;
    }

    public async Task<MessageDto> CreateMessageAsync(CreateMessageDto createMessageDto)
    {
        var senderId = _context.Request.GetUserIdHeaderOrThrow();

        await _conversationRepository.CheckIfEntityExist(createMessageDto.RecipientConversationId);
        await _userRepository.CheckIfEntityExist(senderId);

        var message = MessageFactory.CreateMessage(createMessageDto, senderId);

        await _messageRepository.CreateAsync(message);
        await _messageRepository.SaveChangesAsync();

        var messageDto = _mapper.Map<MessageDto>(message);

        return messageDto;
    }
}