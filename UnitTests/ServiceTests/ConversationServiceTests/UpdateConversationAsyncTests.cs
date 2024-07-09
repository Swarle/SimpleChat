using System.Net;
using FluentAssertions;
using Moq;
using SimpleChat.BL.DTO;
using SimpleChat.BL.Infrastructure;
using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Entities;

namespace UnitTests.ServiceTests.ConversationServiceTests;

[TestFixture]
public class UpdateConversationAsyncTests : BasicConversationServiceTest
{
    [Test]
    public void UpdateConversationAsync_WhenTagAndTitleEmpty_ShouldReturnCompleteTask()
    {
        //Arrange
        var updateDto = new UpdateConversationDto
        {
            Id = Guid.NewGuid(),
        };
        
        //Act
        var result = conversationService.UpdateConversationAsync(updateDto);
        result.Wait();
        
        //Assert
        result.Status.Should().Be(TaskStatus.RanToCompletion);
    }
    
    [Test]
    public void UpdateConversationAsync_WhenUnauthorized_ShouldThrowHttpException()
    {
        //Arrange
        var updateDto = GetUpdateConversationDto();

        userRepository.Setup(e => e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);
        
        //Act 
        var exception = Assert.ThrowsAsync<HttpException>(async () => await conversationService.UpdateConversationAsync(updateDto));
        
        //Assert
        exception.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public void UpdateConversationAsync_WhenConversationNotExist_ShouldThrowHttpException()
    {
        //Arrange
        var updateDto = GetUpdateConversationDto();
        var user = GetUser();
        
        userRepository.Setup(e => e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        conversationRepository.Setup(e =>
            e.GetFirstOrDefaultAsync(It.IsAny<BaseSpecification<Conversation>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Conversation);
        
        //Act
        var exception = Assert.ThrowsAsync<HttpException>(async () => await conversationService.UpdateConversationAsync(updateDto));
        
        //Assert
        exception.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Test]
    public void UpdateConversationAsync_WhenNotMember_ShouldThrowHttpException()
    {
        //Arrange
        var updateDto = GetUpdateConversationDto();
        var user = GetUser();
        var conversation = GetConversationWithMember(user.Id);
        conversation.Members = new List<Member>();
        
        userRepository.Setup(e => e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        conversationRepository.Setup(e =>
                e.GetFirstOrDefaultAsync(It.IsAny<BaseSpecification<Conversation>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversation);
        
        //Act
        var exception = Assert.ThrowsAsync<HttpException>(async () => await conversationService.UpdateConversationAsync(updateDto));
        
        //Assert
        exception.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Test]
    public void UpdateConversationAsync_WhenHasNoPermission_ShouldThrowHttpException()
    {
        //Arrange
        var updateDto = GetUpdateConversationDto();
        var user = GetUser();
        var conversation = GetConversationWithMember(user.Id);
        
        userRepository.Setup(e => e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        conversationRepository.Setup(e =>
                e.GetFirstOrDefaultAsync(It.IsAny<BaseSpecification<Conversation>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversation);
        
        //Act
        var exception = Assert.ThrowsAsync<HttpException>(async () => await conversationService.UpdateConversationAsync(updateDto));
        
        //Assert
        exception.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Test]
    public void UpdateConversationAsync_WhenWasUpdated_ShouldReturnCompletedTask()
    {
        //Arrange
        var updateDto = GetUpdateConversationDto();
        var user = GetUser();
        var conversation = GetConversationWithMember(user.Id);
        conversation.Members.First().UserRole = MemberRole.Admin;
        
        userRepository.Setup(e => e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        conversationRepository.Setup(e =>
                e.GetFirstOrDefaultAsync(It.IsAny<BaseSpecification<Conversation>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversation);
        
        //Act
        var result = conversationService.UpdateConversationAsync(updateDto);
        result.Wait();
        
        //Assert
        result.Status.Should().Be(TaskStatus.RanToCompletion);
    }
    
    private static UpdateConversationDto GetUpdateConversationDto() =>
        new UpdateConversationDto
        {
            Id = Guid.NewGuid(),
            Title = "Sample title",
            Tag = "sample_tag"
        };
}