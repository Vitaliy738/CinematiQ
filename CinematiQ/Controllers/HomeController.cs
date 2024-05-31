using CinematiQ.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CinematiQ.Data;
using CinematiQ.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CinematiQ.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationIdentityUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _context.ApplicationIdentityUser
                .AsNoTracking()
                .Include(u => u.MovieMarkers)
                .Include(u => u.LastWatchedMovies)
                .ThenInclude(m => m.Movie)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }
            
            user.LastWatchedMovies = user.LastWatchedMovies
                .OrderByDescending(lwm => lwm.Date)
                .GroupBy(lwm => lwm.Movie.Id)
                .Select(g => g.First())
                .Take(5)
                .ToList();

            return View(user);
        }

        
        public async Task<IActionResult> Favorite()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var favorites = await _context
                .MovieMarkers
                .AsNoTracking()
                .Where(m => m.User == user && m.Type == MovieMarkerType.Favorite)
                .Include(m => m.Movie)
                .ToListAsync();
            
            return View(favorites);
        }
        
        public async Task<IActionResult> Watching()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var favorites = await _context
                .MovieMarkers
                .AsNoTracking()
                .Where(m => m.User == user && m.Type == MovieMarkerType.Watching)
                .Include(m => m.Movie)
                .ToListAsync();
            
            return View(favorites);
        }
        
        public async Task<IActionResult> Planned()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var favorites = await _context
                .MovieMarkers
                .AsNoTracking()
                .Where(m => m.User == user && m.Type == MovieMarkerType.Planned)
                .Include(m => m.Movie)
                .ToListAsync();
            
            return View(favorites);
        }
        
        public async Task<IActionResult> Viewed()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var favorites = await _context
                .MovieMarkers
                .AsNoTracking()
                .Where(m => m.User == user && m.Type == MovieMarkerType.Viewed)
                .Include(m => m.Movie)
                .ToListAsync();
            
            return View(favorites);
        }
        
        public async Task<IActionResult> Postponed()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var favorites = await _context
                .MovieMarkers
                .AsNoTracking()
                .Where(m => m.User == user && m.Type == MovieMarkerType.Postponed)
                .Include(m => m.Movie)
                .ToListAsync();
            
            return View(favorites);
        }
        
        public async Task<IActionResult> Abandoned()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var favorites = await _context
                .MovieMarkers
                .AsNoTracking()
                .Where(m => m.User == user && m.Type == MovieMarkerType.Abandoned)
                .Include(m => m.Movie)
                .ToListAsync();
            
            return View(favorites);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if (statuscode == 404)
            {
                return View("NotFound");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
