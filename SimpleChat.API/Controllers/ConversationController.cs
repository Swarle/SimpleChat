using Microsoft.AspNetCore.Mvc;
using SimpleChat.BL.DTO;
using SimpleChat.BL.Interfaces;

namespace SimpleChat.API.Controllers;

public class ConversationController : BaseApiController
{
    private readonly IConversationService _conversationService;

    public ConversationController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ConversationSearchDto>>> SearchAsync([FromQuery] string query, CancellationToken cancellationToken)
    {
        var conversationsDto = await _conversationService.GetConversationByTitleOrTagAsync(query, cancellationToken);

        return Ok(conversationsDto);
    }
    
    [HttpGet("{conversationId:guid}")]
    public async Task<ActionResult> GetConversationByIdAsync([FromRoute]Guid conversationId, CancellationToken cancellationToken)
    {
        var conversationDto = await _conversationService.GetConversationByIdAsync(conversationId, cancellationToken);

        return Ok(conversationDto);
    }

    [HttpPost("update")]
    public async Task<ActionResult> UpdateAsync([FromBody]UpdateConversationDto updateConversationDto, CancellationToken cancellationToken)
    {
        await _conversationService.UpdateConversationAsync(updateConversationDto, cancellationToken);

        return Ok();
    }


}