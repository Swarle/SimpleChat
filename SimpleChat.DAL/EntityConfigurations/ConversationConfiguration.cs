using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleChat.DAL.Entities;

namespace SimpleChat.DAL.EntityConfigurations;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.HasIndex(e => e.Tag).IsUnique();
        
        builder.Property(e => e.Title).HasMaxLength(50);
        builder.Property(e => e.Tag).HasMaxLength(30);
    }
}