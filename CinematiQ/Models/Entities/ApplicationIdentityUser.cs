using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CinematiQ.Models.Entities
{
    public class ApplicationIdentityUser : IdentityUser
    {
        public override string Id { get; set; }
        public string UserName { get; set; }
        public string Location { get; set; }
        public List<MovieMarker> MovieMarkers { get; set; }
        public List<LastWatchedMovie> LastWatchedMovies { get; set; }
        public List<Comment> Comments { get; set; }
        
        [Display(Name = "Оцінки сюжету")]
        public List<PlotReview> PlotReviews { get; set; }
        
        [Display(Name = "Оцінки якості картинки")]
        public List<PictureQualityReview> PictureQualityReviews { get; set; }
        
        [Display(Name = "Оцінки персонажів")]
        public List<CharacterReview> CharacterReviews { get; set; }
        
        [Display(Name = "Оцінки власних вражень")]
        public List<PersonalImpressionsReview> PersonalImpressionsReviews { get; set; }
        
        public string Icon { get; set; }
    }
}
