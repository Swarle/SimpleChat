using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SimpleChat.BL.Helpers;
using SimpleChat.BL.Infrastructure;
using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Entities;
using Utility.Constants;

namespace UnitTests.ServiceTests.ConversationServiceTests;

[TestFixture]
public class GetAllUserConversationsAsyncTests : BasicConversationServiceTest
{
    [Test]
    public void GetAllUserConversationsAsync_WhenUnauthorized_ShouldThrowHubExtension()
    {
        //Arrange
        context.Request.Headers.Remove(SD.UserIdHeaderKey);

        //Act & Assert
        var exception =
            Assert.ThrowsAsync<HubException>(async () => await conversationService.GetAllUserConversationsAsync());

        exception.Message.Should().NotBeNullOrWhiteSpace();
    }
    
    [Test]
    public void GetAllUserConversationsAsync_WhenUserNotExist_ShouldThrowHubOperationExtension()
    {
        //Arrange
        userRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);

        //Act & Assert
        var exception =
            Assert.ThrowsAsync<HubOperationException>(async () => await conversationService.GetAllUserConversationsAsync());

        exception.Message.Should().NotBeNullOrWhiteSpace();
    }
    
    [Test]
    public async Task GetAllUserConversationsAsync_WhenConversationsEmpty_ShouldReturnEmptyList()
    {
        //Arrange
        var user = GetUser();
        
        userRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        conversationRepository.Setup(e =>
                e.GetAllAsync(It.IsAny<BaseSpecification<Conversation>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        //Act
        var result = await conversationService.GetAllUserConversationsAsync();
        
        //Assert
        result.Should().BeEmpty();
    }
    
    [Test]
    public async Task GetAllUserConversationsAsync_WhenConversationsExists_ShouldListWithGroupNames()
    {
        //Arrange
        var user = GetUser();
        var conversation = GetConversationsList();
        var groupList = conversation.Select(c => c.Id.ToString()).ToList();
        
        userRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        conversationRepository.Setup(e =>
                e.GetAllAsync(It.IsAny<BaseSpecification<Conversation>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversation);

        //Act
        var result = await conversationService.GetAllUserConversationsAsync();
        
        //Assert
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(groupList);
    }
}