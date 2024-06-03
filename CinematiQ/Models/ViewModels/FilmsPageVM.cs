using CinematiQ.Models.Entities;

namespace CinematiQ.Models.ViewModels;

public class FilmsPageVM
{
    public List<Movie> Movies { get; set; }
    public List<Movie> Series { get; set; }
    public List<Movie> Cartoons { get; set; }
    public List<Movie> Anime { get; set; }
}