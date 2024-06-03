using System.ComponentModel.DataAnnotations;

namespace CinematiQ.Models.Entities;

public class Genre
{
    [Required]
    [Display(Name = "ID")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [Display(Name = "Назва жанру")]
    public string Title { get; set; }
    
    [Required]
    [Display(Name = "Опис жанру")]
    public string Description { get; set; }
}