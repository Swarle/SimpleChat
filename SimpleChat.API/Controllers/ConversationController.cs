using SimpleChat.BL.Interfaces;

namespace SimpleChat.API.Controllers;

public class ConversationController : BaseApiController
{
    private readonly IConversationService _conversationService;

    public ConversationController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }
    
    
}