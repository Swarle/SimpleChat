using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleChat.DAL.Entities;

namespace SimpleChat.DAL.EntityConfigurations;

public class UserToConversationConfiguration : IEntityTypeConfiguration<UserToConversation>
{
    public void Configure(EntityTypeBuilder<UserToConversation> builder)
    {
        builder.HasIndex(e => new { e.UserId, e.ConversationId }).IsUnique();
        
        builder.Property(e => e.UserRole)
            .HasConversion(
                v => v.ToString(),
                v => (ConversationRole)Enum.Parse(typeof(ConversationRole), v));
        
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