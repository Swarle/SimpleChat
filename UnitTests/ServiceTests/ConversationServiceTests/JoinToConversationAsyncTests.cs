using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SimpleChat.BL.Helpers;
using SimpleChat.DAL.Entities;
using Utility.Constants;

namespace UnitTests.ServiceTests.ConversationServiceTests;

[TestFixture]
public class JoinToConversationAsyncTests : BasicConversationServiceTest
{
    [Test]
    public void JoinToConversationAsync_WhenUnauthorized_ShouldThrowHubExtension()
    {
        //Arrange
        var conversationId = Guid.NewGuid();
        context.Request.Headers.Remove(SD.UserIdHeaderKey);
        
        //Act & Assert
        Assert.ThrowsAsync<HubException>(async () => await conversationService.JoinToConversationAsync(conversationId));
    }

    [Test]
    public void JoinToConversationAsync_WhenUserNotExist_ShouldThrowHubOperationException()
    {
        //Arrange
        var conversationId = Guid.NewGuid();
        var user = GetUser();
        
        userRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        //Act & Assert
        Assert.ThrowsAsync<HubOperationException>(async () =>
            await conversationService.JoinToConversationAsync(conversationId));
    }

    [Test]
    public void JoinToConversationAsync_WhenConversationNotExist_ShouldThrowHubOperationException()
    {
        //Arrange
        var conversationId = Guid.NewGuid();
        var user = GetUser();
        
        userRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        conversationRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Conversation);
        
        //Act & Assert
        Assert.ThrowsAsync<HubOperationException>(async () =>
            await conversationService.JoinToConversationAsync(conversationId));
    }

    [Test]
    public async Task JoinToConversationAsync_WhenWasConnected_ShouldReturnUsername()
    {
        //Arrange
        var conversation = GetConversation();
        var user = GetUser();
        
        userRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        conversationRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversation);
        
        //Act
        var result = await conversationService.JoinToConversationAsync(conversation.Id);
        
        //Assert
        conversationRepository.Verify(e => e.UpdateAsync(It.IsAny<Conversation>()));
        conversationRepository.Verify(e => e.SaveChangesAsync());
        result.Should().NotBeNullOrWhiteSpace();
        result.Should().Be(user.Name);
    }
}