using System.Net;
using FluentAssertions;
using Moq;
using SimpleChat.BL.DTO;
using SimpleChat.BL.Infrastructure;
using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Entities;

namespace UnitTests.ServiceTests.ConversationServiceTests;

[TestFixture]
public class GetConversationByIdAsyncTests : BasicConversationServiceTest
{
    [Test]
    public void GetConversationByIdAsync_WhenConversationNotExist_ShouldThrowHttpException()
    {
        //Arrange
        var conversationId = Guid.NewGuid();

        conversationRepository.Setup(e =>
                e.GetFirstOrDefaultAsync(It.IsAny<BaseSpecification<Conversation>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Conversation);
        
        //Act & Assert
        var exception = Assert.ThrowsAsync<HttpException>(
            async () => await conversationService.GetConversationByIdAsync(conversationId));
        
        exception.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [Test]
    public async Task GetConversationByIdAsync_WhenConversationWasFound_ShouldReturnConversationDto()
    {
        //Arrange
        var conversation = GetFullConversation();
        var conversationDto = GetConversationDto();
        
        conversationRepository.Setup(e =>
                e.GetFirstOrDefaultAsync(It.IsAny<BaseSpecification<Conversation>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversation);
        
        //Act
        var result = await conversationService.GetConversationByIdAsync(conversation.Id);
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(conversationDto);
    }
    
    private static Conversation GetFullConversation() =>
        new Conversation
        {
            Id = new Guid("905212D5-CBE5-493F-AABC-96251E28A567"),
            Title = "Sample Titile",
            Tag = "sample_tag",
            Members = new List<Member>
            {
                new Member
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("1CE54B50-95CF-43CE-ACD7-4A117869E5FB"),
                    ConversationId = new Guid("905212D5-CBE5-493F-AABC-96251E28A567"),
                    UserRole = MemberRole.Admin,
                    User = new User
                    {
                        Id = new Guid("1CE54B50-95CF-43CE-ACD7-4A117869E5FB"),
                        Name = "Sample Admin"
                    }
                },
                new Member
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("E7E913F2-3B23-4AF6-901F-50DD37DACFBD"),
                    ConversationId = new Guid("905212D5-CBE5-493F-AABC-96251E28A567"),
                    UserRole = MemberRole.Member,
                    User = new User
                    {
                        Id = new Guid("E7E913F2-3B23-4AF6-901F-50DD37DACFBD"),
                        Name = "Sample Member"
                    }
                }
            },
            Messages = new List<Message>
            {
                new Message
                {
                    Id = new Guid("91158977-5DFD-4578-BD1E-673F910C645F"),
                    ReceiverConversationId = new Guid("905212D5-CBE5-493F-AABC-96251E28A567"),
                    SendAt = DateTime.Parse("2024-07-08 18:11:02.5570508"),
                    SenderId = new Guid("1CE54B50-95CF-43CE-ACD7-4A117869E5FB"),
                    Text = "Sample text"
                },
                new Message
                {
                    Id = new Guid("30B7D113-5D05-46E9-97B2-CD8A3C31E6B5"),
                    ReceiverConversationId = new Guid("905212D5-CBE5-493F-AABC-96251E28A567"),
                    SendAt = DateTime.Parse("2024-07-08 18:11:02.5570508"),
                    SenderId = new Guid("1CE54B50-95CF-43CE-ACD7-4A117869E5FB"),
                    Text = "Sample text 2"
                }
            }
        };

    private static ConversationDto GetConversationDto() =>
        new ConversationDto
        {
            Id = new Guid("905212D5-CBE5-493F-AABC-96251E28A567"),
            Title = "Sample Titile",
            Tag = "sample_tag",
            Members = new List<MemberDto>
            {
                new MemberDto
                {
                    Id = new Guid("1CE54B50-95CF-43CE-ACD7-4A117869E5FB"),
                    MemberRole = "Admin",
                    Name = "Sample Admin"
                },
                new MemberDto
                {
                    Id = new Guid("E7E913F2-3B23-4AF6-901F-50DD37DACFBD"),
                    MemberRole = "Member",
                    Name = "Sample Member"
                },
            },
            Messages = new List<MessageDto>
            {
                new MessageDto
                {
                    Id = new Guid("91158977-5DFD-4578-BD1E-673F910C645F"),
                    ReceiverConversationId = new Guid("905212D5-CBE5-493F-AABC-96251E28A567"),
                    SendAt = DateTime.Parse("2024-07-08 18:11:02.5570508"),
                    SenderId = new Guid("1CE54B50-95CF-43CE-ACD7-4A117869E5FB"),
                    Text = "Sample text"
                },
                new MessageDto
                {
                    Id = new Guid("30B7D113-5D05-46E9-97B2-CD8A3C31E6B5"),
                    ReceiverConversationId = new Guid("905212D5-CBE5-493F-AABC-96251E28A567"),
                    SendAt = DateTime.Parse("2024-07-08 18:11:02.5570508"),
                    SenderId = new Guid("1CE54B50-95CF-43CE-ACD7-4A117869E5FB"),
                    Text = "Sample text 2"
                }
            }
        };
}