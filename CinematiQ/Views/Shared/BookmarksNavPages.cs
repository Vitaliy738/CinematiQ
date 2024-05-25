using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinematiQ.Views.Shared;

public static class BookmarksNavPages
{
    public static string Favorite => "Favorite";
    public static string FavoriteNavClass(ViewContext viewContext) => PageNavClass(viewContext, Favorite);
    
    public static string Planned => "Planned";
    public static string PlannedNavClass(ViewContext viewContext) => PageNavClass(viewContext, Planned);
    
    public static string Watching => "Watching";
    public static string WatchingNavClass(ViewContext viewContext) => PageNavClass(viewContext, Watching);
    
    public static string Viewed => "Viewed";
    public static string ViewedNavClass(ViewContext viewContext) => PageNavClass(viewContext, Viewed);
    
    public static string Postponed => "Postponed";
    public static string PostponedNavClass(ViewContext viewContext) => PageNavClass(viewContext, Postponed);
    
    public static string Abandoned => "Abandoned";
    public static string AbandonedNavClass(ViewContext viewContext) => PageNavClass(viewContext, Abandoned);

    public static string PageNavClass(ViewContext viewContext, string page)
    {
        var activePage = viewContext.ViewData["ActivePage"] as string
                         ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
        return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "bookmark-link-active" : null;
    }
}