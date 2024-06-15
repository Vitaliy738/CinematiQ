using CinematiQ.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinematiQ.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder.HasOne(c => c.Receiver)
            .WithMany(m => m.Notifications)
            .HasForeignKey(r => r.ReceiverId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(c => c.Sender)
            .WithMany()
            .HasForeignKey(c => c.SenderId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}