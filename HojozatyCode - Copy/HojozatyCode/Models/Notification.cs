public class Notification
{
    public string Id { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public DateTime Timestamp { get; set; }
    public Color StatusColor { get; set; }
    public string IconSource { get; set; }
}