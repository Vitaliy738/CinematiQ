namespace CinematiQ.Models.Entities;

public class Episode
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? Trailer { get; set; }
    public string? Thumbnail { get; set; }
    public string SeasonId { get; set; }
    public Season Season { get; set; }
}