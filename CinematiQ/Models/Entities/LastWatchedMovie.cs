using System.ComponentModel.DataAnnotations;

namespace CinematiQ.Models.Entities;

public class LastWatchedMovie
{
    [Required]
    [Display(Name = "ID")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [Display(Name = "ID фільму")]
    public string MovieId { get; set; }
    
    [Required]
    [Display(Name = "Фільм, який було переглянуто")]
    public Movie Movie { get; set; }
    
    [Required]
    [Display(Name = "ID користувача")]
    public string UserId { get; set; }
    
    [Required]
    [Display(Name = "Користувач")]
    public ApplicationIdentityUser User { get; set; }
    
    [Required]
    [Display(Name = "Дата перегляду")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
}