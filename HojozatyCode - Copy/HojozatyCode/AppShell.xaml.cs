
namespace HojozatyCode
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Pages.LoginSignupPage), typeof(Pages.LoginSignupPage));
            Routing.RegisterRoute(nameof(Pages.LogInPage), typeof(Pages.LogInPage));
            Routing.RegisterRoute(nameof(Pages.SignUpPage), typeof(Pages.SignUpPage));
            Routing.RegisterRoute(nameof(Pages.ProfileInfo), typeof(Pages.ProfileInfo));
            Routing.RegisterRoute(nameof(Pages.HomePage), typeof(Pages.HomePage));
            Routing.RegisterRoute(nameof(Pages.FavouritePage), typeof(Pages.FavouritePage));
            Routing.RegisterRoute(nameof(Pages.AccountPage), typeof(Pages.AccountPage));
            Routing.RegisterRoute(nameof(Pages.EditProfile), typeof(Pages.EditProfile));
            Routing.RegisterRoute(nameof(Pages.NotificationsPage), typeof(Pages.NotificationsPage));
            Routing.RegisterRoute(nameof(Pages.SpaceInformationPage), typeof(Pages.SpaceInformationPage));
            Routing.RegisterRoute(nameof(Pages.SpacePicturesPage), typeof(Pages.SpacePicturesPage));
            Routing.RegisterRoute(nameof(Pages.SpaceTypeSelectionPage), typeof(Pages.SpaceTypeSelectionPage));
            Routing.RegisterRoute(nameof(Pages.ServicesPage), typeof(Pages.ServicesPage));
            Routing.RegisterRoute(nameof(Pages.CategoryVenuesPage), typeof(Pages.CategoryVenuesPage));
            Routing.RegisterRoute(nameof(Pages.SpacePolicies), typeof(Pages.SpacePolicies));
            Routing.RegisterRoute(nameof(Pages.ReviewPage), typeof(Pages.ReviewPage));
            Routing.RegisterRoute(nameof(Pages.SuccessPage), typeof(Pages.SuccessPage));
            Routing.RegisterRoute(nameof(Pages.FiltersPage), typeof(Pages.FiltersPage));
            Routing.RegisterRoute(nameof(Pages.AdminApprovalPage), typeof(Pages.AdminApprovalPage));
            Routing.RegisterRoute(nameof(Pages.AdminPanel), typeof(Pages.AdminPanel));
            Routing.RegisterRoute(nameof(Pages.MySpace), typeof(Pages.MySpace));
            Routing.RegisterRoute(nameof(Pages.EditPage), typeof(Pages.EditPage));
            Routing.RegisterRoute(nameof(Pages.ChoosingHallBooking), typeof(Pages.ChoosingHallBooking));
            Routing.RegisterRoute(nameof(Pages.BookingCalendarPage), typeof(Pages.BookingCalendarPage));
            Routing.RegisterRoute(nameof(Pages.ServicesToAdd), typeof(Pages.ServicesToAdd));
            Routing.RegisterRoute(nameof(Pages.ReviewBooking), typeof(Pages.ReviewBooking));
            Routing.RegisterRoute(nameof(Pages.ForgotPasswordPage), typeof(Pages.ForgotPasswordPage));
            Routing.RegisterRoute(nameof(Pages.EnterOTPPage), typeof(Pages.EnterOTPPage));
            Routing.RegisterRoute(nameof(Pages.ResetPasswordPage), typeof(Pages.ResetPasswordPage));
            Routing.RegisterRoute(nameof(Pages.MyBookings), typeof(Pages.MyBookings));
            Routing.RegisterRoute(nameof(Pages.EditBooking), typeof(Pages.EditBooking));

            GoToAsync(nameof(Pages.LoginSignupPage));

        }
    }
}