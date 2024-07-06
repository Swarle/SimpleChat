using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleChat.DAL.Entities;

namespace SimpleChat.DAL.EntityConfigurations;

public class ConnectionConfiguration : IEntityTypeConfiguration<Connection>
{
    public void Configure(EntityTypeBuilder<Connection> builder)
    {
        builder.Property(e => e.ConnectionId).HasMaxLength(200);
        
        builder.HasOne(e => e.User)
            .WithOne(e => e.Connection)
            .HasForeignKey<Connection>(e => e.Id);
    }
}