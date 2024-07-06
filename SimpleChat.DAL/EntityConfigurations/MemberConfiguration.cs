using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleChat.DAL.Entities;

namespace SimpleChat.DAL.EntityConfigurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasIndex(e => new { e.UserId, e.ConversationId }).IsUnique();
        
        builder.Property(e => e.UserRole)
            .HasMaxLength(50)
            .HasConversion(
                v => v.ToString(),
                v => (MemberRole)Enum.Parse(typeof(MemberRole), v));
        
        builder.HasOne(e => e.User)
            .WithMany(e => e.Conversations)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Conversation)
            .WithMany(e => e.Members)
            .HasForeignKey(e => e.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}