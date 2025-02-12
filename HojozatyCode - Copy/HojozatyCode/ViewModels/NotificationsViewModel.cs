using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace HojozatyCode.ViewModels
{
    public partial class NotificationsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Notification> notifications;

        public NotificationsViewModel()
        {
            LoadNotifications();
        }

        private void LoadNotifications()
        {
            Notifications = new ObservableCollection<Notification>
            {
                new Notification 
                { 
                    Id = "1",
                    Type = NotificationType.NewBooking,
                    Title = "You have new booking request",
                    Subtitle = "tap to see",
                    Timestamp = DateTime.Now,
                    StatusColor = Colors.Pink,
                    IconSource = "down.png"
                },
                new Notification 
                { 
                    Id = "2",
                    Type = NotificationType.BookingApproved,
                    Title = "Booking approved !",
                    Subtitle = "tap to review",
                    Timestamp = DateTime.Now,
                    StatusColor = Colors.Green,
                    IconSource = "approve.png"
                },
                // Add more notifications here
            };
        }

        [RelayCommand]
        private async Task NotificationTapped(Notification notification)
        {
            // Handle notification tap based on type
            switch (notification.Type)
            {
                case NotificationType.NewBooking:
                    // await HandleNewBooking(notification);
                    break;
                // Handle other types
            }
        }

        private async Task HandleNewBooking(Notification notification)
        {
            // Navigate to booking details or perform action
            // await Shell.Current.GoToAsync($"bookingDetails?id={notification.Id}");
        }

        [RelayCommand]
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}