using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using SimpleChat.API.Constants;
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
        await _connectionService.CreateConnection(Context.ConnectionId);

        await ConnectToConversations();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _connectionService.DeleteConnection();
        
        await base.OnDisconnectedAsync(exception);
    }

    private async Task ConnectToConversations()
    {
        var groupNames = await _conversationService.GetAllUserConversationsAsync();
    
        foreach (var groupName in groupNames)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }
    
    public async Task CreateConversation(CreateConversationDto conversationDto)
    {
        var groupName = await _conversationService.CreateConversation(conversationDto);

        await ConnectToConversation(groupName);
    }

    private async Task ConnectToConversation(string groupName)
    {
        if (!Guid.TryParse(groupName, out var conversationId))
            throw new HubException("Unable to convert group name to Guid type");

        var connections = await _connectionService.GetConnectionsByConversationId(conversationId);

        foreach (var connection in connections)
        {
            await Groups.AddToGroupAsync(connection, groupName);
        }
    }
    
    public async Task SendMessageToGroup(CreateMessageDto createMessageDto)
    {
        var message = await _messageService.CreateMessage(createMessageDto);
        
        var groupName = message.ReceiverConversationId.ToString();

        await Clients.Groups(groupName).SendAsync(HubMethodNames.NewMessage, message);
    }
}

