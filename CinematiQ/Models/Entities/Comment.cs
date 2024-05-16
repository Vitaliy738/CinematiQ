using System.ComponentModel.DataAnnotations;

namespace CinematiQ.Models.Entities
{
    public class Comment
    {
        [Required]
        [Display(Name = "ID")]
        public string Id { get; set; }
        
        [Required]
        [Display(Name = "Вміст коментаря")]
        public string Content { get; set; }
        
        [Required]
        [Display(Name = "Дата публікації коментаря")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        [Required]
        [Display(Name = "ID фільму до якого опубліковано коментар")]
        public string MovieId { get; set; }
        
        [Required]
        [Display(Name = "Фільм до якого опубліковано коментар")]
        public Movie Movie { get; set; }
        
        [Required]
        [Display(Name = "ID користувача, який опублікував коментар")]
        public string UserId { get; set;}
        
        [Required]
        [Display(Name = "Користувач, який опублікував коментар")]
        public ApplicationIdentityUser User { get; set;}
    }
}
