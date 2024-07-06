using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SimpleChat.BL.DTO;
using SimpleChat.BL.Extensions;
using SimpleChat.BL.Helpers;
using SimpleChat.BL.Interfaces;
using SimpleChat.DAL.Entities;
using SimpleChat.DAL.Repository;
using SimpleChat.DAL.Specifications;

namespace SimpleChat.BL.Services;

public class ConversationService : IConversationService
{
    private readonly IRepository<Conversation> _conversationRepository;
    private readonly IRepository<User> _userRepository;
    private readonly HttpContext _context;

    public ConversationService(IRepository<Conversation> conversationRepository,
        IRepository<User> userRepository,
        IHttpContextAccessor accessor)
    {
        _conversationRepository = conversationRepository;
        _userRepository = userRepository;
        _context = accessor.HttpContext;
    }

    public async Task<string> CreateConversation(CreateConversationDto conversationDto)
    {
        var adminId = _context.Request.GetUserIdHeaderOrThrow();

        await _userRepository.CheckIfEntityExist(adminId);
        
        var conversation = ConversationFactory.CreateConversation(conversationDto.Title);

        var admin = MemberFactory.CreateAdmin(adminId, conversation.Id);

        var members = new List<Member> { admin };
        
        if (conversationDto.MembersId is not null)
        {
            var userSpecification = new UsersByIdsSpecification(conversationDto.MembersId);

            var isUsersExist = await _userRepository.AnyAsync(userSpecification);

            if (!isUsersExist)
                throw new HubException("Some of users in member list does not exist");
            
            members.AddRange(conversationDto.MembersId
                .Select(memberId => MemberFactory.CreateMember(memberId, conversation.Id)));
        }
        
        conversation.Members = members;
        
        await _conversationRepository.CreateAsync(conversation);
        await _conversationRepository.SaveChangesAsync();

        var groupName = conversation.Id.ToString();
        
        return groupName;
    }

    public async Task<List<string>> GetAllUserConversationsAsync()
    {
        var userId = _context.Request.GetUserIdHeaderOrThrow();

        await _userRepository.CheckIfEntityExist(userId);
        
        var conversationSpecification = new ConversationByUserIdSpecification(userId);

        var conversations = await _conversationRepository.GetAllAsync(conversationSpecification);

        var conversationsGroupNames = conversations.Select(c => c.Id.ToString()).ToList();

        return conversationsGroupNames;
    }
    
}