using CinematiQ.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinematiQ.Configurations;

public class MovieMarkerConfiguration : IEntityTypeConfiguration<MovieMarker>
{
    public void Configure(EntityTypeBuilder<MovieMarker> builder)
    {
        builder.HasKey(m => new {m.UserId, m.MovieId});
        
        builder.HasOne(l => l.Movie)
            .WithMany()
            .HasForeignKey(m => m.MovieId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(m => m.User)
            .WithMany(u => u.MovieMarkers)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}