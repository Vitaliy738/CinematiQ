using System.ComponentModel.DataAnnotations;

namespace CinematiQ.Models.Entities;

public class PersonalImpressionsReview
{
    [Required]
    [Display(Name = "ID")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [Display(Name = "Оцінка")]
    public int Grade { get; set; }
    
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