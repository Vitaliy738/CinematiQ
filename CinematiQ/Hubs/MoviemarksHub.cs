using System.Security.Claims;
using CinematiQ.Data;
using CinematiQ.Models.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CinematiQ.Hubs;

public class MoviemarksHub : Hub
{
    private readonly ApplicationDbContext _context;

    public MoviemarksHub(ApplicationDbContext context)
    {
        _context = context;
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
}