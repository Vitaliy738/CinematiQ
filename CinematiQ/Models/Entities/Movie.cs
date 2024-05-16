using System.ComponentModel.DataAnnotations;

namespace CinematiQ.Models.Entities
{
    public enum MovieType
    {
        Anime,
        Movie,
        Serial,
        Cartoon
    }

    public enum MovieStatus
    {
        Announced,
        Release,
        Upcoming
    }

    public class Movie
    {
        [Required]
        [Display(Name = "ID")]
        public string Id { get; set; }
        
        [Required]
        [Display(Name = "Назва")]
        public string Title { get; set; }
        
        [Display(Name = "Альтернативна назва")]
        public string? AlterTitle { get; set; }
        
        [Required]
        [Display(Name = "Постер")]
        public string Poster { get; set; }
        
        public string? Thumbnail { get; set; }
        
        [Display(Name = "Опис")]
        public string Description { get; set; }
        
        [Display(Name = "Короткий опис")]
        public string? ShortDescription { get; set; }
        
        [Display(Name = "Оцінки сюжету")]
        public List<PlotReview>? PlotReviews { get; set; }
        
        [Display(Name = "Оцінки якості картинки")]
        public List<PictureQualityReview>? PictureQualityReviews { get; set; }
        
        [Display(Name = "Оцінки персонажів")]
        public List<CharacterReview>? CharacterReviews { get; set; }
        
        [Display(Name = "Оцінки власних вражень")]
        public List<PersonalImpressionsReview>? PersonalImpressionsReviews { get; set; }
        
        [Display(Name = "Жанри")]
        public List<Genre>? Genres { get; set; }
        
        [Display(Name = "Сезони")]
        public List<Season>? Seasons { get; set; }
        
        [Required]
        [Display(Name = "Рік релізу")]
        public int YearOfRelease { get; set; }
        
        [Display(Name = "Країни")]
        public List<Country>? Countries { get; set; }
        
        [Required]
        [Display(Name = "Студія")]
        public string Studio { get; set; }
        
        [Required]
        [Display(Name = "Категорія фільму")]
        public MovieType MovieType { get; set; }
        
        [Required]
        [Display(Name = "Статус фільму")]
        public MovieStatus MovieStatus { get; set; }
        
        [Display(Name = "Трейлер")]
        public string? Trailer { get; set; }
        
        [Display(Name = "Коментарі")]
        public List<Comment>? Comments { get; set; }
    }
}
