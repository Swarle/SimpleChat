using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SimpleChat.BL.Helpers;
using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Entities;
using Utility.Constants;

namespace UnitTests.ServiceTests.ConversationServiceTests;

[TestFixture]
public class CreateConversationTests : BasicConversationServiceTest
{
    [Test]
    public void CreateConversationAsync_WhenUserUnauthorized_ShouldThrowHubException()
    {
        //Arrange
        var conversationDto = GetCreateConversationDto();

        context.Request.Headers.Remove(SD.UserIdHeaderKey);

        //Act & Assert
        Assert.ThrowsAsync<HubException>(async () =>
            await conversationService.CreateConversationAsync(conversationDto));
    }

    [Test]
    public void CreateConversationAsync_WhenUserNotExist_ShouldThrowHubOperationException()
    {
        //Arrange
        var conversationDto = GetCreateConversationDto();

        userRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);

        //Act & Assert
        Assert.ThrowsAsync<HubOperationException>(async () =>
            await conversationService.CreateConversationAsync(conversationDto));
    }

    [Test]
    public void CreateConversationAsync_WhenConversationAlreadyExist_ShouldThrowHubOperationException()
    {
        //Arrange
        var conversationDto = GetCreateConversationDto();
        var user = GetUser();

        userRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        conversationRepository.Setup(e =>
                e.AnyAsync(It.IsAny<BaseSpecification<Conversation>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        //Act & Assert
        Assert.ThrowsAsync<HubOperationException>(async () =>
            await conversationService.CreateConversationAsync(conversationDto));
    }
    
    [Test]
    public void CreateConversationAsync_WhenUserInMemberListNotExist_ShouldReturnGroupName()
    {
        //Arrange
        var conversationDto = GetCreateConversationDto();
        var user = GetUser();

        userRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        userRepository.Setup(e =>
                e.AnyAsync(It.IsAny<BaseSpecification<User>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        conversationRepository.Setup(e =>
                e.AnyAsync(It.IsAny<BaseSpecification<Conversation>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        //Act & Assert
        Assert.ThrowsAsync<HubOperationException>(async () => await conversationService.CreateConversationAsync(conversationDto));
    }

    [Test]
    public async Task CreateConversationAsync_WhenMemberListEmpty_ShouldReturnGroupName()
    {
        //Arrange
        var conversationDto = GetCreateConversationDto();
        conversationDto.MembersId = null;
        var user = GetUser();

        userRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        conversationRepository.Setup(e =>
                e.AnyAsync(It.IsAny<BaseSpecification<Conversation>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        //Act
        var result = await conversationService.CreateConversationAsync(conversationDto);
        
        //Assert
        conversationRepository.Verify(e => e.CreateAsync(It.IsAny<Conversation>()));
        conversationRepository.Verify(e => e.SaveChangesAsync());
        result.Should().NotBeNullOrWhiteSpace();
        Guid.TryParse(result, out var id).Should().BeTrue();
        id.Should().NotBe(Guid.Empty);
    }
    
    [Test]
    public async Task CreateConversationAsync_WhenUserInMemberListExist_ShouldReturnGroupName()
    {
        //Arrange
        var conversationDto = GetCreateConversationDto();
        var user = GetUser();

        userRepository.Setup(e =>
                e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        userRepository.Setup(e =>
                e.AnyAsync(It.IsAny<BaseSpecification<User>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        conversationRepository.Setup(e =>
                e.AnyAsync(It.IsAny<BaseSpecification<Conversation>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        //Act
        var result = await conversationService.CreateConversationAsync(conversationDto);
        
        //Assert
        conversationRepository.Verify(e => e.CreateAsync(It.IsAny<Conversation>()));
        conversationRepository.Verify(e => e.SaveChangesAsync());
        result.Should().NotBeNullOrWhiteSpace();
        Guid.TryParse(result, out var id).Should().BeTrue();
        id.Should().NotBe(Guid.Empty);
    }
}