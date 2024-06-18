using CinematiQ.Data;
using CinematiQ.Hubs;
using CinematiQ.Models;
using CinematiQ.Models.Entities;
using CinematiQ.Models.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace CinematiQ.Controllers;

public class FilmsController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ApplicationIdentityUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<NotificationHub> _notificationHub;

    public FilmsController(ILogger<HomeController> logger, UserManager<ApplicationIdentityUser> userManager, ApplicationDbContext context, IHubContext<NotificationHub> notificationHub)
    {
        _logger = logger;
        _userManager = userManager;
        _context = context;
        _notificationHub = notificationHub;
    }
    
    public async Task<IActionResult> Index()
    {
        FilmsPageVM filmsPage = new FilmsPageVM();
        filmsPage.Movies = await _context.Movies
            .AsNoTracking()
            .Where(m => m.MovieType == MovieType.Movie)
            .Take(6)
            .ToListAsync();
        
        filmsPage.Series = await _context.Movies
            .AsNoTracking()
            .Where(m => m.MovieType == MovieType.Serial)
            .Take(6)
            .ToListAsync();
        
        filmsPage.Cartoons = await _context.Movies
            .AsNoTracking()
            .Where(m => m.MovieType == MovieType.Cartoon)
            .Take(6)
            .ToListAsync();
        
        filmsPage.Anime = await _context.Movies
            .AsNoTracking()
            .Where(m => m.MovieType == MovieType.Anime)
            .Take(6)
            .ToListAsync();
        
        return View(filmsPage);
    }
    
    public async Task<IActionResult> Movies(int pageNumber = 1, int pageSize = 18)
    {
        var movies = await _context.Movies
            .AsNoTracking()
            .Where(m => m.MovieType == MovieType.Movie)
            .ToPagedListAsync(pageNumber, pageSize);

        if (!movies.Any())
        {
            return NotFound();
        }

        return View(movies);
    }
        
    public async Task<IActionResult> Series(int pageNumber = 1, int pageSize = 18)
    {
        var series = await _context.Movies
            .AsNoTracking()
            .Where(m => m.MovieType == MovieType.Serial)
            .ToPagedListAsync(pageNumber, pageSize);
        
        if (!series.Any())
        {
            return NotFound();
        }
            
        return View(series);
    }
        
    public async Task<IActionResult> Cartoons(int pageNumber = 1, int pageSize = 18)
    {
        var cartoons = await _context.Movies
            .AsNoTracking()
            .Where(m => m.MovieType == MovieType.Cartoon)
            .ToPagedListAsync(pageNumber, pageSize);
        
        if (!cartoons.Any())
        {
            return NotFound();
        }
            
        return View(cartoons);
    }
        
    public async Task<IActionResult> Anime(int pageNumber = 1, int pageSize = 18)
    {
        var anime = await _context.Movies
            .AsNoTracking()
            .Where(m => m.MovieType == MovieType.Anime)
            .ToPagedListAsync(pageNumber, pageSize);
        
        if (!anime.Any())
        {
            return NotFound();
        }
            
        return View(anime);
    }

    public async Task<IActionResult> Announcements(int pageNumber = 1, int pageSize = 18)
    {
        var announcements = await _context.Movies
            .AsNoTracking()
            .Where(m => m.MovieStatus == MovieStatus.Announced)
            .ToPagedListAsync(pageNumber, pageSize);
        
        if (!announcements.Any())
        {
            return NotFound();
        }
            
        return View(announcements);
    }
    
    public async Task<IActionResult> Releases(int pageNumber = 1, int pageSize = 18)
    {
        var releases = await _context.Movies
            .AsNoTracking()
            .Where(m => m.MovieStatus == MovieStatus.Release)
            .OrderBy(m => m.YearOfRelease)
            .ToPagedListAsync(pageNumber, pageSize);
        
        if (!releases.Any())
        {
            return NotFound();
        }
            
        return View(releases);
    }

    public async Task<IActionResult> Film(string id)
    {
        FilmPageVM pageVm = new FilmPageVM();

        var movie = await _context.Movies
            .Include(m => m.Countries)
            .Include(m => m.Genres)
            .Include(m => m.Seasons)
            .Include(m => m.PlotReviews)
            .Include(m => m.CharacterReviews)
            .Include(m => m.PictureQualityReviews)
            .Include(m => m.PersonalImpressionsReviews)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null || movie.Id != id)
        {
            return NotFound();
        }

        movie.Comments = await _context.Comments
            .Include(c => c.User)
            .Where(c => c.MovieId == movie.Id)
            .OrderByDescending(c => c.Date)
            .ToListAsync();

        pageVm.Movie = movie;
        pageVm.SimilarMovies = _context.Movies.AsNoTracking().Take(4).ToList();

        if (!User.Identity?.IsAuthenticated ?? true)
        {
            return View(pageVm);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var lastWatchedMovie = await _context.LastWatchedMovies.FirstOrDefaultAsync(
            lvm => lvm.User == user && lvm.Movie == movie);

        DateTime now = DateTime.Now;

        if (lastWatchedMovie == null) // if not found
        {
            await _context.LastWatchedMovies.AddAsync(new LastWatchedMovie
            {
                Movie = movie,
                User = user,
                Date = now,
            });
        }
        else
        {
            lastWatchedMovie.Date = now;
        }

        await _context.SaveChangesAsync();

        var moviemarkType =
            await _context.MovieMarkers.FirstOrDefaultAsync(m => m.Movie == movie && m.User == user);

        if (moviemarkType != null)
        {
            pageVm.SelectedMoviemarker = (int)moviemarkType.Type;
        }
        
        return View(pageVm);
    }

    public async Task<IActionResult> Search(string query, int pageNumber = 1, int pageSize = 18)
    {
        if (string.IsNullOrEmpty(query))
        {
            return NotFound();
        }

        var searchVm = new SearchVM();

        searchVm.Movies = await _context.Movies
            .Where(m => m.Title.Contains(query) ||
                        m.Description.Contains(query))
            .ToPagedListAsync(pageNumber, pageSize);

        if (!searchVm.Movies.Any())
        {
            return NotFound();
        }

        searchVm.SearchQuery = query;
        
        return View(searchVm);
    }

}