using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CinematiQ.Models.Entities
{
    public class ApplicationIdentityUser : IdentityUser
    {
        [MaxLength(50)]
        public string? Name { get; set; }
        
        [MaxLength(50)]
        public string? Location { get; set; }
        
        [MaxLength(50)]
        public string? Status { get; set; }

        public bool PublicLastWatchedMovies { get; set; } = true;
        
        public bool PublicMovieMarkers { get; set; } = true;

        public bool PublicStatus { get; set; } = true;
        
        public List<MovieMarker> MovieMarkers { get; set; } = [];
        
        public List<LastWatchedMovie> LastWatchedMovies { get; set; } = [];
        
        public List<Comment> Comments { get; set; } = [];
        
        public List<PlotReview> PlotReviews { get; set; } = [];
        
        public List<PictureQualityReview> PictureQualityReviews { get; set; } = [];
        
        public List<CharacterReview> CharacterReviews { get; set; } = [];
        
        public List<PersonalImpressionsReview> PersonalImpressionsReviews { get; set; } = [];
        
        public string Icon { get; set; }

        public List<Notification> Notifications { get; set; } = [];
    }
}
