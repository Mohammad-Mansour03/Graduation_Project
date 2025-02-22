//This class to deals with the notification page
public class Notification
{
    //Store the notification ID
    public string Id { get; set; }

    //Store the type of the notification
    public NotificationType Type { get; set; }

    //Store the main title of the notification
    public string Title { get; set; }
    //Store the details of the nitfication
    public string Subtitle { get; set; }
    //Store the date for the notification
    public DateTime Timestamp { get; set; }
    //Store the Color that will give for the notification
    public Color StatusColor { get; set; }
    //Store the photo icon that will appear with the notification
    public string IconSource { get; set; }
}