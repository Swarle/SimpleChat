using System.ComponentModel.DataAnnotations;

namespace SimpleChat.BL.DTO;

public class UpdateConversationDto
{
    public Guid Id { get; set; }
    [MaxLength(50)]
    public string? Title { get; set; }
    [RegularExpression(@"^[a-z_]{1,30}$")]
    public string? Tag { get; set; }
}