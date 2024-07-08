namespace SimpleChat.BL.DTO;

public class MemberDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string MemberRole { get; set; }
}