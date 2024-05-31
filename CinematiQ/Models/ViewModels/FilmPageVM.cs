using CinematiQ.Models.Entities;

namespace CinematiQ.Models;

public class FilmPageVM
{
    public Movie Movie { get; set; } = new();
    public int SelectedMoviemarker { get; set; } = -1;

    public string? IsSelected(int number)
    {
        return SelectedMoviemarker == number ? "selected" : null;
    }
}