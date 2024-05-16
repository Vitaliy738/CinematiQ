using CinematiQ.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinematiQ.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder.HasMany(m => m.Genres)
            .WithMany();
        
        builder.HasMany(m => m.Countries)
            .WithMany();
        
        builder.HasMany(m => m.Comments)
            .WithOne(r => r.Movie)
            .HasForeignKey(m => m.MovieId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}