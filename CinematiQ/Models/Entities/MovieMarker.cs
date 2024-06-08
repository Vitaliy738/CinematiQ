using System.ComponentModel.DataAnnotations;

namespace CinematiQ.Models.Entities
{
    public enum MovieMarkerType
    {
        Favorite,
        Planned,
        Watching,
        Viewed,
        Postponed,
        Abandoned
    }
    
    public class MovieMarker
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Display(Name = "Категорія закладки")]
        public MovieMarkerType Type { get; set; }
        
        [Required]
        [Display(Name = "Дата додавання")]
        [DataType(DataType.Date)]
        public DateTime AddedDate { get; set; }
        
        [Required]
        [Display(Name = "ID фільму")]
        public string MovieId { get; set; }
        
        [Required]
        [Display(Name = "Фільм")]
        public Movie Movie { get; set; }
        
        [Required]
        [Display(Name = "ID користувача")]
        public string UserId { get; set; }
        
        [Required]
        [Display(Name = "Користувач")]
        public ApplicationIdentityUser User { get; set; }
    }
}