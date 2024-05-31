using System.Security.Claims;
using CinematiQ.Data;
using CinematiQ.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CinematiQ.Hubs;

public class CommentsHub : Hub
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationIdentityUser> _userManager;

    public CommentsHub(ApplicationDbContext context, UserManager<ApplicationIdentityUser> userManager)
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

        await Clients.All.SendAsync("ReceivePostComment", comment.User.UserName, comment.Date, comment.Content);
    }

    // TODO Рома, потрібно зробити видалення комментарів з БД.
    // Я зробив метод для додавання комментарів до БД, але треба зробити ще й видалення.
    // Роби все по аналогії PostComment.
    // Повертати щось на клієнт не потрібно, я сам це зроблю.
    public async Task DeleteComment(string commentId)
    {
        
    }
}