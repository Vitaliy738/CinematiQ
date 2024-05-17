using CinematiQ.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinematiQ.Configurations;

public class CharacterReviewConfiguration : IEntityTypeConfiguration<CharacterReview>
{
    public void Configure(EntityTypeBuilder<CharacterReview> builder)
    {
        builder.HasKey(s => new {s.UserId, s.MovieId});

        builder.HasOne(pr => pr.Movie)
            .WithMany(m => m.CharacterReviews)
            .HasForeignKey(pr => pr.MovieId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(pr => pr.User)
            .WithMany(u => u.CharacterReviews)
            .HasForeignKey(pr => pr.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}