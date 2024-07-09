using FluentAssertions;
using Moq;
using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Entities;

namespace UnitTests.ServiceTests.ConversationServiceTests;

[TestFixture]
public class GetConversationByTitleOrTagAsyncTests : BasicConversationServiceTest
{
    [Test]
    public async Task GetConversationByTitleOrTagAsync_WhenQueryIsEmpty_ShouldReturnEmptyList()
    {
        //Arrange
        const string query = "";

        //Act
        var result = await conversationService
            .GetConversationByTitleOrTagAsync(query, It.IsAny<CancellationToken>());
        
        //Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetConversationByTitleOrTagAsync_WhenConversationWasNotFound_ShouldReturnEmptyList()
    {
        //Arrange
        const string query = "sample";

        conversationRepository.Setup(e =>
                e.GetAllAsync(It.IsAny<BaseSpecification<Conversation>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        //Act
        var result = await conversationService
            .GetConversationByTitleOrTagAsync(query, It.IsAny<CancellationToken>());
        
        //Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetConversationByTitleOrTagAsync_WhenConversationsWasFound_ShouldReturnConversationDtoList()
    {
        //Arrange
        const string query = "sample";
        var conversationList = GetConversationsList();
        var conversationSearchDtoList = GetConversationSearchDtoList();
        
        conversationRepository.Setup(e =>
                e.GetAllAsync(It.IsAny<BaseSpecification<Conversation>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversationList);

        //Act
        var result = await conversationService
            .GetConversationByTitleOrTagAsync(query, It.IsAny<CancellationToken>());
        
        //Assert
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(conversationSearchDtoList);
    }
}