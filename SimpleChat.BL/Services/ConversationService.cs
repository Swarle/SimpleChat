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

    public async Task<string> CreateConversationAsync(CreateConversationDto conversationDto)
    {
        var adminId = _context.Request.GetUserIdHeaderOrThrow();

        await _userRepository.CheckIfEntityExist(adminId);
        
        var conversation = ConversationFactory.CreateConversation(conversationDto.Title);

        var admin = MemberFactory.CreateAdmin(adminId, conversation.Id);

        var members = new List<Member> { admin };
        
        if (conversationDto.MembersId is not null)
        {
            var userSpecification = new UserByIdSpecification(conversationDto.MembersId);

            var isUsersExist = await _userRepository.AnyAsync(userSpecification);

            if (!isUsersExist)
                throw new HubOperationException("Some of users in member list does not exist");
            
            members.AddRange(conversationDto.MembersId
                .Select(memberId => MemberFactory.CreateMember(memberId, conversation.Id)));
        }
        
        conversation.Members = members;
        
        await _conversationRepository.CreateAsync(conversation);
        await _conversationRepository.SaveChangesAsync();

        var groupName = conversation.Id.ToString();
        
        return groupName;
    }

    public async Task<string> JoinToConversationAsync(Guid conversationId)
    {
        var userId = _context.Request.GetUserIdHeaderOrThrow();

        var user = await _userRepository.CheckIfEntityExist(userId);

        var conversation = await _conversationRepository.CheckIfEntityExist(conversationId);

        var member = MemberFactory.CreateMember(userId, conversationId);
        
        conversation.Members.Add(member);
        await _conversationRepository.UpdateAsync(conversation);

        await _conversationRepository.SaveChangesAsync();

        var username = user.Name;

        return username;
    }

    public async Task<string> DeleteConversationAsync(Guid conversationId)
    {
        var userId = _context.Request.GetUserIdHeaderOrThrow();

        var userSpecification = new UserByIdSpecification(userId, conversationId);

        var user = await _userRepository.GetFirstOrDefaultAsync(userSpecification) ?? 
                   throw new HubOperationException("The user with this id either does not exist or is not a member of the group");

        var member = user.Conversations.First(c => c.ConversationId == conversationId);

        if (member.UserRole != MemberRole.Admin)
            throw new HubOperationException("There are no permissions to do the operation");

        var conversation = member.Conversation;

        await _conversationRepository.DeleteAsync(conversation);
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