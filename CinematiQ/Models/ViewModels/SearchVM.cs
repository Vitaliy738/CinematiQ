namespace CinematiQ.Models.ViewModels;

public class SearchVM
{
    public X.PagedList.IPagedList<CinematiQ.Models.Entities.Movie> Movies { get; set; }
    public string? SearchQuery { get; set; }
}