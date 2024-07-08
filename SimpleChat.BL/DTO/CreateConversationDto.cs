using System.ComponentModel.DataAnnotations;
using Utility.Constants;

namespace SimpleChat.BL.DTO;

public class CreateConversationDto
{
    public List<Guid>? MembersId { get; set; }
    [MaxLength(50)]
    public required string Title { get; set; }
    [RegularExpression(@"^[a-z_]{1,30}$")]
    public required string Tag { get; set; }
}