using System.ComponentModel.DataAnnotations;
using CinematiQ.Data;
using CinematiQ.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinematiQ.Areas.Identity.Pages.Account.Manage;

public class PrivacyModel : PageModel
{
    private readonly UserManager<ApplicationIdentityUser> _userManager;
    private readonly SignInManager<ApplicationIdentityUser> _signInManager;
    private readonly ApplicationDbContext _context;

    public PrivacyModel(UserManager<ApplicationIdentityUser> userManager, 
        SignInManager<ApplicationIdentityUser> signInManager, 
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    [TempData]
    public string StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        public bool PublicLastWatchedMovies { get; set; }
        
        public bool PublicMovieMarkers { get; set; }
        
        public bool PublicStatus { get; set; }
    }

    private async Task LoadAsync(ApplicationIdentityUser user)
    {
        Input = new InputModel
        {
            PublicLastWatchedMovies = user.PublicLastWatchedMovies,
            PublicMovieMarkers = user.PublicMovieMarkers,
            PublicStatus = user.PublicStatus
        };
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        if (Input.PublicLastWatchedMovies != user.PublicLastWatchedMovies)
        {
            user.PublicLastWatchedMovies = Input.PublicLastWatchedMovies;
        }
            
        if (Input.PublicMovieMarkers != user.PublicMovieMarkers)
        {
            user.PublicMovieMarkers = Input.PublicMovieMarkers;
        }
        
        if (Input.PublicStatus != user.PublicStatus)
        {
            user.PublicStatus = Input.PublicStatus;
        }
            
        await _context.SaveChangesAsync();
        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "«м≥ни було усп≥шно збережено";
        return RedirectToPage();
    }
}