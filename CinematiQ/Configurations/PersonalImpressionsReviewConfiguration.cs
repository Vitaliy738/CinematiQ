using CinematiQ.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinematiQ.Configurations;

public class PersonalImpressionsReviewConfiguration : IEntityTypeConfiguration<PersonalImpressionsReview>
{
    public void Configure(EntityTypeBuilder<PersonalImpressionsReview> builder)
    {
        builder.HasKey(s => new {s.UserId, s.MovieId});

        builder.HasOne(pr => pr.Movie)
            .WithMany(m => m.PersonalImpressionsReviews)
            .HasForeignKey(pr => pr.MovieId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(pr => pr.User)
            .WithMany(u => u.PersonalImpressionsReviews)
            .HasForeignKey(pr => pr.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}