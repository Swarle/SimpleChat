using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SimpleChat.BL.Helpers;
using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Entities;
using Utility.Constants;

namespace UnitTests.ServiceTests.ConversationServiceTests;

[TestFixture]
public class DeleteConversationAsyncTests : BasicConversationServiceTest
{
    [Test]
    public void DeleteConversationAsync_WhenUnauthorized_ShouldThrowHubException()
    {
        //Arrange
        var conversationId = Guid.NewGuid();
        context.Request.Headers.Remove(SD.UserIdHeaderKey);
        
        //Act & Assert
        Assert.ThrowsAsync<HubException>(async () => await conversationService.DeleteConversationAsync(conversationId));
    }

    [Test]
    public void DeleteConversationAsync_WhenUserNotExist_ShouldThrowHubOperationException()
    {
        //Arrange
        var conversationId = Guid.NewGuid();

        userRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);
        
        //Act & Assert
        Assert.ThrowsAsync<HubOperationException>(async () =>
            await conversationService.DeleteConversationAsync(conversationId));
    }

    [Test]
    public void DeleteConversationAsync_WhenConversationNotExist_ShouldThrowHubOperationException()
    {
        //Arrange
        var conversationId = Guid.NewGuid();
        var user = GetUser();
        
        userRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        //Act & Assert
        Assert.ThrowsAsync<HubOperationException>(async () =>
            await conversationService.DeleteConversationAsync(conversationId));
    }
    
    [Test]
    public void DeleteConversationAsync_WhenNotAMember_ShouldThrowHubOperationException()
    {
        //Arrange
        var user = GetUser();
        var conversation = GetConversationWithMember(user.Id);
        conversation.Members = new List<Member>();
        
        userRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        conversationRepository.Setup(e =>
                e.GetFirstOrDefaultAsync(It.IsAny<BaseSpecification<Conversation>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversation);
        
        //Act & Assert
        Assert.ThrowsAsync<HubOperationException>(async () =>
            await conversationService.DeleteConversationAsync(conversation.Id));
    }
    
    [Test]
    public void DeleteConversationAsync_WhenHasNoPermission_ShouldThrowHubOperationException()
    {
        //Arrange
        var user = GetUser();
        var conversation = GetConversationWithMember(user.Id);
        
        userRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        conversationRepository.Setup(e =>
                e.GetFirstOrDefaultAsync(It.IsAny<BaseSpecification<Conversation>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversation);
        
        //Act & Assert
        Assert.ThrowsAsync<HubOperationException>(async () =>
            await conversationService.DeleteConversationAsync(conversation.Id));
    }
    
    [Test]
    public async Task DeleteConversationAsync_WhenWasDeleted_ShouldReturnGroupName()
    {
        //Arrange
        var user = GetUser();
        var conversation = GetConversationWithMember(user.Id);
        conversation.Members.First().UserRole = MemberRole.Admin;
        
        userRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        conversationRepository.Setup(e =>
                e.GetFirstOrDefaultAsync(It.IsAny<BaseSpecification<Conversation>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversation);
        
        //Act
        var result = await conversationService.DeleteConversationAsync(conversation.Id);
        
        //Assert
        result.Should().NotBeNullOrWhiteSpace();
        Guid.TryParse(result, out var id).Should().BeTrue();
        id.Should().Be(conversation.Id);
        conversationRepository.Verify(e => e.DeleteAsync(It.IsAny<Conversation>()));
        conversationRepository.Verify(e => e.SaveChangesAsync());
    }
}