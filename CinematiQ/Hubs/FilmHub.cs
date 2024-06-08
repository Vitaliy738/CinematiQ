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
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

        if (comment != null)
            _context.Remove(comment);

        await _context.SaveChangesAsync();
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
                Type = moviemarkType,
                AddedDate = DateTime.Now
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
            plotReview = new PlotReview
            {
                User = user,
                Movie = movie,
                Grade = value
            };
            
            user.PlotReviews.Add(plotReview);
        }
        else
        {
            plotReview.Grade = value;
        }
        
        await _context.SaveChangesAsync();

        var newPlotRating = await _context.PlotReviews.Where(pr => pr.Movie == movie).AverageAsync(p => p.Grade);
        
        await Clients.All.SendAsync("ReceiveSetPlotRating", newPlotRating, plotReview.MovieId);
    }
    public async Task SetCharacterRating(string movieId, int value)
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

        var characterReview = await _context.CharacterReviews.FirstOrDefaultAsync(p => p.User == user && p.Movie == movie);
        if (characterReview == null)
        {
            characterReview = new CharacterReview()
            {
                User = user,
                Movie = movie,
                Grade = value
            };
            
            user.CharacterReviews.Add(characterReview);
        }
        else
        {
            characterReview.Grade = value;
        }
        
        await _context.SaveChangesAsync();

        var newCharacterRating = await _context.CharacterReviews.Where(cr => cr.Movie == movie).AverageAsync(p => p.Grade);
        
        await Clients.All.SendAsync("ReceiveSetCharacterRating", newCharacterRating, characterReview.MovieId);
    }
    public async Task SetPictureRating(string movieId, int value)
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

        var pictureReview = await _context.PictureQualityReviews.FirstOrDefaultAsync(p => p.User == user && p.Movie == movie);
        if (pictureReview == null)
        {
            pictureReview = new PictureQualityReview()
            {
                User = user,
                Movie = movie,
                Grade = value
            };
            
            user.PictureQualityReviews.Add(pictureReview);
        }
        else
        {
            pictureReview.Grade = value;
        }
        
        await _context.SaveChangesAsync();

        var newPictureRating = await _context.PictureQualityReviews.Where(pr => pr.Movie == movie).AverageAsync(p => p.Grade);
        
        await Clients.All.SendAsync("ReceiveSetPictureRating", newPictureRating, pictureReview.MovieId);
    }
    public async Task SetPersonalRating(string movieId, int value)
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

        var personalReview = await _context.PersonalImpressionsReviews.FirstOrDefaultAsync(p => p.User == user && p.Movie == movie);
        if (personalReview == null)
        {
            personalReview = new PersonalImpressionsReview()
            {
                User = user,
                Movie = movie,
                Grade = value
            };
            
            user.PersonalImpressionsReviews.Add(personalReview);
        }
        else
        {
            personalReview.Grade = value;
        }
        
        await _context.SaveChangesAsync();

        var newPersonalRating = await _context.PersonalImpressionsReviews.Where(pi => pi.Movie == movie).AverageAsync(p => p.Grade);
        
        await Clients.All.SendAsync("ReceiveSetPersonalRating", newPersonalRating, personalReview.MovieId);
    }
}