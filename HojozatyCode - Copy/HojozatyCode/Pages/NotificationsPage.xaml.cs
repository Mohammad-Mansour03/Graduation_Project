using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages
{
    public partial class NotificationsPage : ContentPage
    {
        public NotificationsPage()
        {
            InitializeComponent();
            BindingContext = new NotificationsViewModel();
        }
    }
}