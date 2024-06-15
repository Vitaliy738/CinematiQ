using CinematiQ.Configurations;
using CinematiQ.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CinematiQ.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationIdentityUser> ApplicationIdentityUser { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<LastWatchedMovie> LastWatchedMovies { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieMarker> MovieMarkers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CharacterReview> CharacterReviews { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<PersonalImpressionsReview> PersonalImpressionsReviews { get; set; }
        public DbSet<PictureQualityReview> PictureQualityReviews { get; set; }
        public DbSet<PlotReview> PlotReviews { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new LastWatchedMovieConfiguration());
            builder.ApplyConfiguration(new MovieConfiguration());
            builder.ApplyConfiguration(new MovieMarkerConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());
            builder.ApplyConfiguration(new CharacterReviewConfiguration());
            builder.ApplyConfiguration(new EpisodeConfiguration());
            builder.ApplyConfiguration(new PersonalImpressionsReviewConfiguration());
            builder.ApplyConfiguration(new PictureQualityReviewConfiguration());
            builder.ApplyConfiguration(new PlotReviewConfiguration());
            builder.ApplyConfiguration(new SeasonConfiguration());
            builder.ApplyConfiguration(new NotificationConfiguration());
            
            base.OnModelCreating(builder);
        }
    }
}
