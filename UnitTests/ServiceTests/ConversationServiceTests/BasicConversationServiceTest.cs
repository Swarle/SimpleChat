using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using SimpleChat.BL.DTO;
using SimpleChat.BL.Interfaces;
using SimpleChat.BL.Services;
using SimpleChat.DAL.Entities;
using SimpleChat.DAL.Repository;
using Utility.Constants;

namespace UnitTests.ServiceTests.ConversationServiceTests;

public class BasicConversationServiceTest
{
    protected Mock<IRepository<Conversation>> conversationRepository;
    protected Mock<IRepository<User>> userRepository;
    protected DefaultHttpContext context;
    protected IConversationService conversationService;
    private IMapper _mapper;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _mapper = UnitTestHelper.GetMapper();
    }

    [SetUp]
    public void SetUp()
    {
        conversationRepository = new Mock<IRepository<Conversation>>();
        userRepository = new Mock<IRepository<User>>();
        context = new DefaultHttpContext();
        context.Request.Headers[SD.UserIdHeaderKey] = Guid.NewGuid().ToString();
        
        var accessor = new Mock<IHttpContextAccessor>();

        accessor.Setup(e => e.HttpContext).Returns(context);
        conversationService = new ConversationService(conversationRepository.Object, userRepository.Object,
            accessor.Object, _mapper);
    }
    
    public static User GetUser() =>
        new User
        {
            Id = Guid.NewGuid(),
            Name = "Sample Name"
        };
    
    protected static CreateConversationDto GetCreateConversationDto() =>
        new CreateConversationDto
        {
            MembersId = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
            Title = "SampleTitle",
            Tag = "sample_tag"
        };
    
    protected static Conversation GetConversation() =>
        new Conversation
        {
            Id = new Guid(),
            Tag = "sample_tag",
            Title = "Sample Title",
        };
    
    protected static Conversation GetConversationWithMember(Guid userId)
    {
        var conversation = new Conversation
        {
            Id = Guid.NewGuid(),
            Tag = "sample_tag",
            Title = "Sample Title",
        };
        
        var member = new Member
        {
            UserId = userId,
            ConversationId = conversation.Id,
            Conversation = conversation
        };
        
        conversation.Members.Add(member);
        
        return conversation;
    }
    
    protected static List<Conversation> GetConversationsList() =>
    [
        new Conversation
        {
            Id = new Guid("905212D5-CBE5-493F-AABC-96251E28A567"),
            Tag = "sample_tag",
            Title = "Sample Tag"
        },

        new Conversation
        {
            Id = new Guid("F2C38306-847F-4099-B46C-0A90A123FE55"),
            Tag = "sample_tag2",
            Title = "Sample Tag1"
        }
    ];
    
    protected static IEnumerable<ConversationSearchDto> GetConversationSearchDtoList() =>
    [
        new ConversationSearchDto
        {
            Id = new Guid("905212D5-CBE5-493F-AABC-96251E28A567"),
            Tag = "sample_tag",
            Title = "Sample Tag"
        },

        new ConversationSearchDto
        {
            Id = new Guid("F2C38306-847F-4099-B46C-0A90A123FE55"),
            Tag = "sample_tag2",
            Title = "Sample Tag1"
        }
    ];
    
        
}