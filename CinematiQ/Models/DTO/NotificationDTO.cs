namespace CinematiQ.Models.DTO;

public class NotificationDTO
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Content { get; set; } = "";
    public string Header { get; set; } = "";
    public string? SenderUserName { get; set; } = "";
    public DateTime Date { get; set; } = DateTime.Now;
    public bool IsRead { get; set; } = false;
}