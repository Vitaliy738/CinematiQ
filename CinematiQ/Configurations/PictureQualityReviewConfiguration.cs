using CinematiQ.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinematiQ.Configurations;

public class PictureQualityReviewConfiguration : IEntityTypeConfiguration<PictureQualityReview>
{
    public void Configure(EntityTypeBuilder<PictureQualityReview> builder)
    {
        builder.HasKey(s => new {s.UserId, s.MovieId});

        builder.HasOne(pr => pr.Movie)
            .WithMany(m => m.PictureQualityReviews)
            .HasForeignKey(pr => pr.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pr => pr.User)
            .WithMany(u => u.PictureQualityReviews)
            .HasForeignKey(pr => pr.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}