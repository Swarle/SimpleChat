﻿using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using SimpleChat.BL.DTO;
using SimpleChat.BL.Extensions;
using SimpleChat.BL.Helpers;
using SimpleChat.BL.Infrastructure;
using SimpleChat.BL.Interfaces;
using SimpleChat.DAL.Entities;
using SimpleChat.DAL.Repository;
using SimpleChat.DAL.Specifications;

namespace SimpleChat.BL.Services;

public class ConversationService : IConversationService
{
    private readonly IRepository<Conversation> _conversationRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;
    private readonly HttpContext _context;

    public ConversationService(IRepository<Conversation> conversationRepository,
        IRepository<User> userRepository,
        IHttpContextAccessor accessor,
        IMapper mapper)
    {
        _conversationRepository = conversationRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _context = accessor.HttpContext;
    }

    public async Task<string> CreateConversationAsync(CreateConversationDto conversationDto)
    {
        var adminId = _context.Request.GetUserIdOrThrowHubException();

        await _userRepository.GetByIdOrThrowHubOperationExceptionAsync(adminId);

        var conversationSpecification = new ConversationByTagSpecification(conversationDto.Tag);

        var isConversationExist = await _conversationRepository.AnyAsync(conversationSpecification);

        if (isConversationExist)
            throw new HubOperationException("Conversation with this tag name already exist");
        
        var conversation = ConversationFactory.CreateConversation(conversationDto.Title, conversationDto.Tag);

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
        var userId = _context.Request.GetUserIdOrThrowHubException();

        var user = await _userRepository.GetByIdOrThrowHubOperationExceptionAsync(userId);

        var conversation = await _conversationRepository.GetByIdOrThrowHubOperationExceptionAsync(conversationId);

        var member = MemberFactory.CreateMember(userId, conversationId);
        
        conversation.Members.Add(member);
        
        await _conversationRepository.UpdateAsync(conversation);
        await _conversationRepository.SaveChangesAsync();

        var username = user.Name;

        return username;
    }

    public async Task<string> DeleteConversationAsync(Guid conversationId)
    {
        var userId = _context.Request.GetUserIdOrThrowHubException();

        await _userRepository.GetByIdOrThrowHubOperationExceptionAsync(userId);

        var conversationSpecification = new ConversationWithMembersSpecification(conversationId, userId);

        var conversation = await _conversationRepository.GetFirstOrDefaultAsync(conversationSpecification) ??
                           throw new HubOperationException("Conversation with given id does not exist");

        var member = conversation.Members.FirstOrDefault();

        if (member is null)
            throw new HubOperationException("The user is not a member of this group");

        if (member.UserRole != MemberRole.Admin)
            throw new HubOperationException("There are no permissions to do the operation");

        await _conversationRepository.DeleteAsync(conversation);
        await _conversationRepository.SaveChangesAsync();

        var groupName = conversation.Id.ToString();

        return groupName;
    }

    public async Task<IEnumerable<ConversationSearchDto>> GetConversationByTitleOrTagAsync(string query, CancellationToken cancellationToken = default)
    {
        if (query.IsNullOrEmpty())
            return new List<ConversationSearchDto>();

        var conversationSpecification = new ConversationByTagOrTitleSpecification(query);
        
        var conversations = await _conversationRepository.GetAllAsync(conversationSpecification, cancellationToken);

        var conversationsDto = _mapper.Map<List<ConversationSearchDto>>(conversations);

        return conversationsDto;
    }

    public async Task<ConversationDto> GetConversationByIdAsync(Guid conversationId, CancellationToken cancellationToken = default)
    {
        var conversationSpecification = new ConversationByIdSpecification(conversationId);

        var conversation = await _conversationRepository.GetFirstOrDefaultAsync(conversationSpecification, cancellationToken) ??
                           throw new HttpException(HttpStatusCode.NotFound, "Conversation with the given id does not exist");

        var conversationDto = _mapper.Map<ConversationDto>(conversation);

        return conversationDto;
    }

    public async Task UpdateConversationAsync(UpdateConversationDto updateConversationDto, CancellationToken cancellationToken = default)
    {
        if(updateConversationDto.Tag == null && updateConversationDto.Title == null)
            return;
        
        var userId = _context.Request.GetUserIdOrThrowHttpException();
        
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken) ??
                throw new HttpException(HttpStatusCode.Unauthorized, "User with given id does not exist");

        var conversationSpecification = new ConversationWithMembersSpecification(updateConversationDto.Id, userId);

        var conversation = await _conversationRepository.GetFirstOrDefaultAsync(conversationSpecification, cancellationToken) ??
                           throw new HttpException(HttpStatusCode.NotFound, "Conversation with given id does not exist");

        var member = conversation.Members.FirstOrDefault();

        if (member is null)
            throw new HttpException(HttpStatusCode.Forbidden, "The user is not a member of this group");
        
        if (member.UserRole != MemberRole.Admin)
            throw new HttpException(HttpStatusCode.Forbidden, "You do not have admin privileges to edit this conversation");

        conversation = _mapper.Map(updateConversationDto, conversation);

        await _conversationRepository.UpdateAsync(conversation);
        await _conversationRepository.SaveChangesAsync();
    }

    public async Task<List<string>> GetAllUserConversationsAsync()
    {
        var userId = _context.Request.GetUserIdOrThrowHubException();

        await _userRepository.GetByIdOrThrowHubOperationExceptionAsync(userId);
        
        var conversationSpecification = new ConversationByUserIdSpecification(userId);

        var conversations = await _conversationRepository.GetAllAsync(conversationSpecification);

        var conversationsGroupNames = conversations.Select(c => c.Id.ToString()).ToList();

        return conversationsGroupNames;
    }
    
}