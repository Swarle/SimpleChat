using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleChat.DAL.Entities;

namespace SimpleChat.DAL.EntityConfigurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.Property(e => e.Text).HasMaxLength(500);

        builder.HasOne(e => e.User)
            .WithMany(e => e.Messages)
            .HasForeignKey(e => e.SenderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Conversation)
            .WithMany(e => e.Messages)
            .HasForeignKey(e => e.ReceiverConversationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}