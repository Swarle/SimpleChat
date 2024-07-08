namespace SimpleChat.BL.DTO;

public class ConversationDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Tag { get; set; }
    public List<MessageDto> Messages { get; set; } = [];
    public List<MemberDto> Members { get; set; } = [];
}