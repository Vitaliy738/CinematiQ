using System.Security.Claims;
using CinematiQ.Data;
using CinematiQ.Models.DTO;
using CinematiQ.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace CinematiQ.Hubs;

public class NotificationHub : Hub
{
    private readonly ApplicationDbContext _context;

    public NotificationHub(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task NotificationToRead(List<string> notificationsId)
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !notificationsId.Any())
        {
            return;
        }
        
        foreach (var notificationId in notificationsId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        await Clients.User(userId).SendAsync("ReceiveNotificationToRead");
    }
    
    public override async Task<Task> OnConnectedAsync()
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.ApplicationIdentityUser
            .Include(u => u.Comments)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null || string.IsNullOrEmpty(userId))
        {
            return base.OnConnectedAsync();
        }

        var userNotifications = await _context.Notifications
            .AsNoTracking()
            .Include(n => n.Receiver)
            .Include(n => n.Sender)
            .Where(n => n.Receiver == user)
            .OrderBy(n => n.Date)
            .Take(8)
            .Select(n => new NotificationDTO()
            {
                Id = n.Id,
                Content = n.Content,
                Header = n.Header,
                SenderUserName = n.Sender.UserName,
                Date = n.Date,
                IsRead = n.IsRead
            })
            .ToListAsync();
        
        await Clients.User(userId).SendAsync("ReceiveUserNotifications", userNotifications);
        
        return base.OnConnectedAsync();
    }
}