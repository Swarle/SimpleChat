namespace SimpleChat.BL.DTO;

public class MessageDto
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverConversationId { get; set; }
    public required string Text { get; set; }
    public DateTime SendAt { get; set; }
}