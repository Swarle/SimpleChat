using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SimpleChat.BL.Extensions;
using SimpleChat.BL.Helpers;
using SimpleChat.BL.Interfaces;
using SimpleChat.DAL.Entities;
using SimpleChat.DAL.Repository;
using SimpleChat.DAL.Specifications;

namespace SimpleChat.BL.Services;

public class ConnectionService : IConnectionService
{
    private readonly IRepository<Connection> _connectionRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Conversation> _conversationRepository;
    private readonly HttpContext _context;
    
    public ConnectionService(IRepository<Connection> connectionRepository,
        IHttpContextAccessor accessor,
        IRepository<User> userRepository,
        IRepository<Conversation> conversationRepository)
    {
        _connectionRepository = connectionRepository;
        _userRepository = userRepository;
        _conversationRepository = conversationRepository;
        _context = accessor.HttpContext;
    }

    public async Task CreateConnectionAsync(string connectionId)
    {
        var userId = _context.Request.GetUserIdOrThrowHubException();

        var user = await _userRepository.GetByIdAsync(userId) ??
            throw new HubException("User with given id does not exist");

        var connection = await _connectionRepository.GetByIdAsync(userId);

        if (connection != null)
            throw new HubException("Connection with this user already exist");

        connection = new Connection { Id = userId, ConnectionId = connectionId };

        await _connectionRepository.CreateAsync(connection);
        await _connectionRepository.SaveChangesAsync();
    }

    public async Task<List<string>> GetConnectionsByConversationIdAsync(Guid conversationId)
    {
        await _conversationRepository.GetByIdOrThrowHubOperationExceptionAsync(conversationId);
        
        var connectionSpecification = new ConnectionByConversationIdSpecification(conversationId);

        var connections = await _connectionRepository.GetAllAsync(connectionSpecification);

        var connectionsId = connections.Select(c => c.ConnectionId).ToList();

        return connectionsId;
    }

    public async Task DeleteConnectionAsync()
    {
        var userId = _context.Request.GetUserIdOrThrowHubException();
        
        await _userRepository.GetByIdOrThrowHubOperationExceptionAsync(userId);

        var connection = await _connectionRepository.GetByIdAsync(userId) ??
                         throw new HubException("Connection with requested user id does not exist");

        await _connectionRepository.DeleteAsync(connection);
        await _connectionRepository.SaveChangesAsync();
    }
}