using CinematiQ.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinematiQ.Configurations;

public class LastWatchedMovieConfiguration : IEntityTypeConfiguration<LastWatchedMovie>
{
    public void Configure(EntityTypeBuilder<LastWatchedMovie> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder.HasOne(l => l.Movie)
            .WithMany()
            .HasForeignKey(l => l.MovieId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(l => l.User)
            .WithMany(u => u.LastWatchedMovies)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}