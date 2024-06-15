using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinematiQ.Data;
using CinematiQ.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;

namespace CinematiQ.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieController(ApplicationDbContext context)
        {
            _context = context;
        }

        class CreateMovieVM
        {
            public Movie Movie { get; set; }
            public List<string> SelectedGenres { get; set; }
            public List<string> SelectedCounties { get; set; }
        }

        // GET: Movie
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 15)
        {
            return View(await _context.Movies.AsNoTracking().ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: Movie/Details/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movie/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.MovieTypeList = GetMovieTypeSelectList();
            ViewBag.MovieStatusList = GetMovieStatusSelectList();
            ViewBag.Genres = new SelectList(_context.Genres.AsNoTracking(), "Id", "Title");
            ViewBag.Countries = new SelectList(_context.Countries.AsNoTracking(), "Id", "Name");
            return View();
        }
        
        private SelectList GetMovieTypeSelectList()
        {
            var values = Enum.GetValues(typeof(MovieType)).Cast<MovieType>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            return new SelectList(values, "Value", "Text");
        }
        
        private SelectList GetMovieStatusSelectList()
        {
            var values = Enum.GetValues(typeof(MovieStatus)).Cast<MovieStatus>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            return new SelectList(values, "Value", "Text");
        }

        // POST: Movie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Title,AlterTitle,Poster,Thumbnail,Description,ShortDescription,YearOfRelease,Studio,MovieType,MovieStatus,Trailer")] Movie movie, string[] genres, string[] countries)
        {
            if (ModelState.IsValid)
            {
                movie.Genres = _context.Genres.Where(g => genres.Contains(g.Id)).ToList();
                movie.Countries = _context.Countries.Where(c => countries.Contains(c.Id)).ToList();
                
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MovieTypeList = GetMovieTypeSelectList();
            ViewBag.MovieStatusList = GetMovieStatusSelectList();
            ViewBag.Genres = new SelectList(_context.Genres.AsNoTracking(), "Id", "Title");
            ViewBag.Countries = new SelectList(_context.Countries.AsNoTracking(), "Id", "Name");
            return View(movie);
        }

        // GET: Movie/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie movie = new Movie();

            movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            
            ViewBag.MovieTypeList = GetMovieTypeSelectList();
            ViewBag.MovieStatusList = GetMovieStatusSelectList();
            ViewBag.Genres = new SelectList(_context.Genres.AsNoTracking(), "Id", "Title");
            ViewBag.Countries = new SelectList(_context.Countries.AsNoTracking(), "Id", "Name");

            return View(movie);
        }

        // POST: Movie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,AlterTitle,Poster,Thumbnail,Description,ShortDescription,YearOfRelease,Studio,MovieType,MovieStatus,Trailer")] Movie movie, string[] genres, string[] countries)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var movieToUpdate = await _context.Movies
                        .Include(m => m.Genres)
                        .Include(m => m.Countries)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (movieToUpdate == null)
                    {
                        return NotFound();
                    }

                    UpdateMovieProperties(movieToUpdate, movie);
                    UpdateMovieRelationships(movieToUpdate, genres, countries);

                    _context.Update(movieToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.MovieTypeList = GetMovieTypeSelectList();
            ViewBag.MovieStatusList = GetMovieStatusSelectList();
            ViewBag.Genres = new SelectList(_context.Genres.AsNoTracking(), "Id", "Title");
            ViewBag.Countries = new SelectList(_context.Countries.AsNoTracking(), "Id", "Name");
            return View(movie);
        }
        
        private void UpdateMovieProperties(Movie targetMovie, Movie sourceMovie)
        {
            targetMovie.Title = sourceMovie.Title;
            targetMovie.AlterTitle = sourceMovie.AlterTitle;
            targetMovie.Poster = sourceMovie.Poster;
            targetMovie.Thumbnail = sourceMovie.Thumbnail;
            targetMovie.Description = sourceMovie.Description;
            targetMovie.ShortDescription = sourceMovie.ShortDescription;
            targetMovie.YearOfRelease = sourceMovie.YearOfRelease;
            targetMovie.Studio = sourceMovie.Studio;
            targetMovie.MovieType = sourceMovie.MovieType;
            targetMovie.MovieStatus = sourceMovie.MovieStatus;
            targetMovie.Trailer = sourceMovie.Trailer;
        }

        private void UpdateMovieRelationships(Movie movie, string[] genreIds, string[] countryIds)
        {
            movie.Genres.Clear();
            movie.Genres = _context.Genres.Where(g => genreIds.Contains(g.Id)).ToList();

            movie.Countries.Clear();
            movie.Countries = _context.Countries.Where(c => countryIds.Contains(c.Id)).ToList();
        }

        // GET: Movie/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(string id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
