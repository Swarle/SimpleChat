namespace SimpleChat.BL.DTO;

public class ConversationSearchDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Tag { get; set; }
}