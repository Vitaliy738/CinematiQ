using System.ComponentModel.DataAnnotations;

namespace CinematiQ.Models.Entities;

public class Notification
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? ReceiverId { get; set; }
    public ApplicationIdentityUser? Receiver { get; set; }
    
    public string? SenderId { get; set; }
    public ApplicationIdentityUser? Sender { get; set; }

    public string Header { get; set; } = "";

    public string Content { get; set; } = "";

    public bool IsRead { get; set; } = false;
    
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Now;
}