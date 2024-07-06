namespace SimpleChat.BL.DTO;

public class CreateConversationDto
{
    public List<Guid>? MembersId { get; set; }
    public required string Title { get; set; }
}