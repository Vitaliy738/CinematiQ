namespace CinematiQ.Models.Entities;

public class Season
{
    public string Id { get; set; }
    public string Title { get; set; }
    public List<Episode> Episodes { get; set; }
    public string MovieId { get; set; }
    public Movie Movie { get; set; }
}