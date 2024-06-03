using System.Security.Claims;
using CinematiQ.Data;
using CinematiQ.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CinematiQ.Hubs;

public class FilmHub : Hub
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationIdentityUser> _userManager;

    public FilmHub(ApplicationDbContext context, UserManager<ApplicationIdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task PostComment(string movieId, string content)
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.ApplicationIdentityUser
            .Include(u => u.Comments)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return;
        }

        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
        
        if (movie == null)
        {
            return;
        }

        Comment comment = new Comment
        {
            Content = content,
            Date = DateTime.Now,
            Movie = movie,
            User = user
        };
        
        user.Comments.Add(comment);

        await _context.SaveChangesAsync();

        await Clients.All.SendAsync("ReceivePostComment", comment.User.UserName, comment.Date, comment.Content, comment.MovieId);
    }

    public async Task DeleteComment(string commentId)
    {
        
    }
    
    public async Task SetBookmark(string movieId, int moviemarkNumber)
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.ApplicationIdentityUser
            .Include(u => u.MovieMarkers)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return;
        }

        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
        if (movie == null)
        {
            return;
        }
        
        var moviemarkType = (MovieMarkerType)Enum.ToObject(typeof(MovieMarkerType), moviemarkNumber);

        var moviemark = await _context.MovieMarkers.FirstOrDefaultAsync(m => m.Movie == movie && m.User == user);
        if (moviemark == null)
        {
            user.MovieMarkers.Add(new MovieMarker
            {
                Movie = movie,
                Type = moviemarkType
            });
        }
        else
        {
            moviemark.Type = moviemarkType;
        }

        await _context.SaveChangesAsync();
        await Clients.All.SendAsync("ReceiveSetBookmark");
    }
    
    public async Task RemoveBookmark(string movieId)
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.ApplicationIdentityUser
            .Include(u => u.MovieMarkers)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return;
        }

        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
        if (movie == null)
        {
            return;
        }
        
        var moviemark = await _context.MovieMarkers.FirstOrDefaultAsync(m => m.Movie == movie && m.User == user);
        if (moviemark != null)
        {
            user.MovieMarkers.Remove(moviemark);
        }
        
        await _context.SaveChangesAsync();
        await Clients.All.SendAsync("ReceiveRemoveBookmark");
    }

    public async Task SetPlotRating(string movieId, int value)
    {
        if (value > 5 || value < 1)
        {
            return;
        }
        
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.ApplicationIdentityUser
            .Include(u => u.PlotReviews)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return;
        }

        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
        if (movie == null)
        {
            return;
        }

        var plotReview = await _context.PlotReviews.FirstOrDefaultAsync(p => p.User == user && p.Movie == movie);
        if (plotReview == null)
        {
            user.PlotReviews.Add(new PlotReview
            {
                Grade = value,
                Movie = movie
            });
        }
        else
        {
            plotReview.Grade = value;
        }
        
        await _context.SaveChangesAsync();

        var newPlotRating = await _context.PlotReviews.AverageAsync(p => p.Grade);
        
        await Clients.All.SendAsync("ReceiveSetPlotRating", newPlotRating, plotReview.MovieId);
    }
}