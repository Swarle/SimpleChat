using System.ComponentModel.DataAnnotations;

namespace SimpleChat.BL.DTO;

public class CreateMessageDto
{
    public Guid RecipientConversationId { get; set; }
    [MaxLength(500)]
    public required string Text { get; set; }
}