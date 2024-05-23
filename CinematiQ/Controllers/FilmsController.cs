using CinematiQ.Data;
using CinematiQ.Models.Entities;
using CinematiQ.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    
    public async Task<IActionResult> Movies()
    {
        var movies = await _context.Movies
            .AsNoTracking()
            .Where(m => m.MovieType == MovieType.Movie)
            .ToListAsync();

        if (movies.Count == 0)
        {
            return NotFound();
        }

        return View(movies);
    }
        
    public async Task<IActionResult> Series()
    {
        var series = await _context.Movies
            .AsNoTracking()
            .Where(m => m.MovieType == MovieType.Serial)
            .ToListAsync();
        
        if (series.Count == 0)
        {
            return NotFound();
        }
            
        return View(series);
    }
        
    public async Task<IActionResult> Cartoons()
    {
        var cartoons = await _context.Movies
            .AsNoTracking()
            .Where(m => m.MovieType == MovieType.Cartoon)
            .ToListAsync();
        
        if (cartoons.Count == 0)
        {
            return NotFound();
        }
            
        return View(cartoons);
    }
        
    public async Task<IActionResult> Anime()
    {
        var anime = await _context.Movies
            .AsNoTracking()
            .Where(m => m.MovieType == MovieType.Anime)
            .ToListAsync();
        
        if (anime.Count == 0)
        {
            return NotFound();
        }
            
        return View(anime);
    }

    public async Task<IActionResult> Announcements()
    {
        var announcements = await _context.Movies
            .AsNoTracking()
            .Where(m => m.MovieStatus == MovieStatus.Announced)
            .ToListAsync();
        
        if (announcements.Count == 0)
        {
            return NotFound();
        }
            
        return View(announcements);
    }
    
    public async Task<IActionResult> Releases()
    {
        var releases = await _context.Movies
            .AsNoTracking()
            .Where(m => m.MovieStatus == MovieStatus.Release)
            .OrderBy(m => m.YearOfRelease)
            .ToListAsync();
        
        if (releases.Count == 0)
        {
            return NotFound();
        }
            
        return View(releases);
    }
}