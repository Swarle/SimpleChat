using Bogus;
using Bogus.Extensions;
using FluentAssertions;
using IntegrationTests.HubTests.Common;
using Microsoft.AspNetCore.SignalR.Client;
using SimpleChat.API.Constants;
using SimpleChat.API.Helpers;
using SimpleChat.API.Hubs;
using SimpleChat.BL.DTO;
using SimpleChat.DAL.Entities;

namespace IntegrationTests.HubTests;

[TestFixture]
public class ConversationHubTests : HubIntegrationTestBase
{
    [Test]
    public async Task SendMessage_WhenSendMessage_ShouldSendMessageToNewMessageMethod()
    {
        //Arrange
        var countdownEvent = new CountdownEvent(2);

        var userList = await GetUserList();
        
        var admin = userList[0];
        var member = userList[1];
        
        var conversationId = await CreateConversation(admin.Id, [member.Id]);
        
        await using var adminConnection = await GetHubConnectionAsync(admin.Id);
        await using var memberConnection = await GetHubConnectionAsync(member.Id);
        
        var messageToSend = new CreateMessageDto
        {
            RecipientConversationId = conversationId,
            Text = "Sample Message"
        };

        MessageDto? receivedMessage = null;
        
        memberConnection.On<MessageDto>(HubMethodNames.NewMessage, msg =>
        {
            receivedMessage = msg;
            countdownEvent.Signal();
        });

        adminConnection.On<MessageDto>(HubMethodNames.NewMessage, _ => countdownEvent.Signal());
        
        //Act
        await memberConnection.InvokeAsync(nameof(ConversationHub.SendMessageToGroup), messageToSend);

        countdownEvent.Wait(1000);
        
        //Assert
        receivedMessage.Should().NotBeNull();
        receivedMessage.ReceiverConversationId.Should().Be(conversationId);
        receivedMessage.SenderId.Should().Be(member.Id);
        receivedMessage.Text.Should().Be(messageToSend.Text);
    }
    
    [Test]
    public async Task SendMessage_WhenMessageNotValid_ShouldSendErrorToErrorMethod()
    {
        //Arrange
        var countdownEvent = new CountdownEvent(1);
        
        var userList = await GetUserList();
        
        var user = userList[0];
        
        await using var userConnection = await GetHubConnectionAsync(user.Id);

        var messageToSend = new CreateMessageDto
        {
            RecipientConversationId = Guid.NewGuid(),
            Text = new Faker().Lorem.Text().ClampLength(min: 600)
        };

        OperationFailedResult? receivedError = null;
        
        userConnection.On<OperationFailedResult>(HubMethodNames.Error, msg =>
        {
            receivedError = msg;
            countdownEvent.Signal();
        });
        
        //Act
        await userConnection.InvokeAsync(nameof(ConversationHub.SendMessageToGroup), messageToSend);

        countdownEvent.Wait(1000);
        
        //Assert
        receivedError.Should().NotBeNull();
        receivedError.ErrorMessage.Should().BeNull();
        receivedError.ErrorMessages.Should().NotBeNull();
    }

    [Test]
    public async Task CreateConversation_WhenNotValid_ShouldSendErrorToErrorMethod()
    {
        //Arrange
        var countdownEvent = new CountdownEvent(1);
        
        var userList = await GetUserList();
        
        var user = userList[0];
        
        await using var userConnection = await GetHubConnectionAsync(user.Id);

        var createConversation = new CreateConversationDto
        {
            Title = "Simple title",
            Tag = "Incorect Tag Name"
        };

        OperationFailedResult? receivedError = null;
        
        userConnection.On<OperationFailedResult>(HubMethodNames.Error, msg =>
        {
            receivedError = msg;
            countdownEvent.Signal();
        });
        
        //Act
        await userConnection.InvokeAsync(nameof(ConversationHub.CreateConversation), createConversation);

        countdownEvent.Wait(1000);
        
        //Assert
        receivedError.Should().NotBeNull();
        receivedError.ErrorMessage.Should().BeNull();
        receivedError.ErrorMessages.Should().NotBeNull();
    }
    
    [Test]
    public async Task CreateConversation_WhenWasCreated_ShouldSendToNewConversationCreatedMethod()
    {
        //Arrange
        var countdownEvent = new CountdownEvent(1);
        
        var userList = await GetUserList();
        
        var user = userList[0];
        
        await using var userConnection = await GetHubConnectionAsync(user.Id);

        var createConversation = new CreateConversationDto
        {
            Title = "Simple title",
            Tag = "simple_tag"
        };

        string? receivedMessage = null;
        
        userConnection.On<string>(HubMethodNames.NewConversationCreated, msg =>
        {
            receivedMessage = msg;
            countdownEvent.Signal();
        });
        
        //Act
        await userConnection.InvokeAsync(nameof(ConversationHub.CreateConversation), createConversation);

        countdownEvent.Wait(1000);
        
        //Assert
        receivedMessage.Should().NotBeNullOrWhiteSpace();
    }
    
    private async Task<Guid> CreateConversation(Guid adminId, List<Guid> membersId)
    {
        var dbContext = GetDbContext();

        var conversation = new Conversation
        {
            Title = "Sample title",
            Tag = "sample_tag",
            Members = new List<Member>
            {
                new Member
                {
                    UserId = adminId,
                    UserRole = MemberRole.Admin
                }
            }
        };

        foreach (var memberId in membersId)
        {
            conversation.Members.Add(new Member
            {
                UserId = memberId,
                UserRole = MemberRole.Member
            });
        }

        await dbContext.Conversations.AddAsync(conversation);
        await dbContext.SaveChangesAsync();

        return conversation.Id;
    }
}