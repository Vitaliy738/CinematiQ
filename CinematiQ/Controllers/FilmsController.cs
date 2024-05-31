using CinematiQ.Data;
using CinematiQ.Models;
using CinematiQ.Models.Entities;
using CinematiQ.Models.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace CinematiQ.Controllers;

public class FilmsController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ApplicationIdentityUser> _userManager;
    private readonly ApplicationDbContext _context;


    public FilmsController(ILogger<HomeController> logger, UserManager<ApplicationIdentityUser> userManager, ApplicationDbContext context)
    {
        _logger = logger;
        _userManager = userManager;
        _context = context;
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

        if (!User.Identity.IsAuthenticated)
        {
            return View(pageVm);
        }
        
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }
            
        user.LastWatchedMovies.Add(new LastWatchedMovie
        {
            Movie = movie,
            Date = DateTime.Now
        });

        _context.Update(user);
        await _context.SaveChangesAsync();
            
            
        var moviemarkType =
            await _context.MovieMarkers.FirstOrDefaultAsync(m => m.Movie == movie && m.User == user);
        
        if (moviemarkType != null)
        {
            pageVm.SelectedMoviemarker = (int)moviemarkType.Type;
        }
        
        return View(pageVm);
    }
}