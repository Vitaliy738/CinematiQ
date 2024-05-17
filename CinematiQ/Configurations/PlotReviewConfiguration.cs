using CinematiQ.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinematiQ.Configurations;

public class PlotReviewConfiguration : IEntityTypeConfiguration<PlotReview>
{
    public void Configure(EntityTypeBuilder<PlotReview> builder)
    {
        builder.HasKey(s => new {s.UserId, s.MovieId});

        builder.HasOne(pr => pr.Movie)
            .WithMany(m => m.PlotReviews)
            .HasForeignKey(pr => pr.MovieId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(pr => pr.User)
            .WithMany(u => u.PlotReviews)
            .HasForeignKey(pr => pr.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}