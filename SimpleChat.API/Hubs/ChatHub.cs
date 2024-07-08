using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using SimpleChat.API.Constants;
using SimpleChat.API.Extensions;
using SimpleChat.BL.DTO;
using SimpleChat.BL.Interfaces;

namespace SimpleChat.API.Hubs;

public class ChatHub : Hub
{
    private readonly IConversationService _conversationService;
    private readonly IConnectionService _connectionService;
    private readonly IMessageService _messageService;
    
    public ChatHub(IConversationService conversationService,
        IConnectionService connectionService,
        IMessageService messageService)
    {
        _conversationService = conversationService;
        _connectionService = connectionService;
        _messageService = messageService;
    }

    public override async Task OnConnectedAsync()
    {
        await _connectionService.CreateConnectionAsync(Context.ConnectionId);

        await ConnectToConversations();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _connectionService.DeleteConnectionAsync();
        
        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task SendMessageToGroup(CreateMessageDto createMessageDto)
    {
        this.ValidateValue(createMessageDto);
        
        var message = await _messageService.CreateMessageAsync(createMessageDto);
        
        var groupName = message.ReceiverConversationId.ToString();

        await Clients.Groups(groupName).SendAsync(HubMethodNames.NewMessage, message);
    }

    public async Task JoinToConversation(Guid conversationId)
    {
        var username = await _conversationService.JoinToConversationAsync(conversationId);

        var groupName = conversationId.ToString();

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        await Clients.Groups(groupName)
            .SendAsync(HubMethodNames.NewMember, $"A User with username {username} joined the group");
    }

    public async Task DeleteConversation(Guid conversationId)
    {
        var connections = await _connectionService.GetConnectionsByConversationIdAsync(conversationId);
        
        var groupName = await _conversationService.DeleteConversationAsync(conversationId);

        await DisconnectFromConversation(groupName, connections);
    }
    
    public async Task CreateConversation(CreateConversationDto conversationDto)
    {
        this.ValidateValue(conversationDto);
        
        var groupName = await _conversationService.CreateConversationAsync(conversationDto);

        await ConnectToConversation(groupName);
    }

    private async Task ConnectToConversations()
    {
        var groupNames = await _conversationService.GetAllUserConversationsAsync();
    
        foreach (var groupName in groupNames)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }

    private async Task ConnectToConversation(string groupName)
    {
        if (!Guid.TryParse(groupName, out var conversationId))
            throw new HubException("Unable to convert group name to Guid type");

        var connections = await _connectionService.GetConnectionsByConversationIdAsync(conversationId);

        foreach (var connection in connections)
        {
            await Groups.AddToGroupAsync(connection, groupName);
        }
    }

    private async Task DisconnectFromConversation(string groupName, List<string> connections)
    {
        foreach (var connection in connections)
        {
            await Groups.RemoveFromGroupAsync(connection, groupName);
        }
    }
    

}

