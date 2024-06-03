using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CinematiQ.Models.Entities
{
    public class ApplicationIdentityUser : IdentityUser
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public List<MovieMarker> MovieMarkers { get; set; } = new();
        public List<LastWatchedMovie> LastWatchedMovies { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        
        [Display(Name = "Оцінки сюжету")]
        public List<PlotReview> PlotReviews { get; set; } = new();
        
        [Display(Name = "Оцінки якості картинки")]
        public List<PictureQualityReview> PictureQualityReviews { get; set; } = new();
        
        [Display(Name = "Оцінки персонажів")]
        public List<CharacterReview> CharacterReviews { get; set; } = new();
        
        [Display(Name = "Оцінки власних вражень")]
        public List<PersonalImpressionsReview> PersonalImpressionsReviews { get; set; } = new();
        
        public string Icon { get; set; }
    }
}
