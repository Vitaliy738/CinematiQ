using System.ComponentModel.DataAnnotations;

namespace CinematiQ.Models.Entities;

public class Country
{
    [Required]
    [Display(Name = "ID")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [Display(Name = "Назва країни")]
    public string Name { get; set; }
}