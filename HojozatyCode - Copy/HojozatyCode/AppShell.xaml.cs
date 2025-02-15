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
            Routing.RegisterRoute(nameof(Pages.ChatList), typeof(Pages.ChatList));
            Routing.RegisterRoute(nameof(Pages.ChatPage), typeof(Pages.ChatPage));
            Routing.RegisterRoute(nameof(Pages.FavouritePage), typeof(Pages.FavouritePage));
            Routing.RegisterRoute(nameof(Pages.AccountPage), typeof(Pages.AccountPage));
            Routing.RegisterRoute(nameof(Pages.AddSpacePage), typeof(Pages.AddSpacePage));
            Routing.RegisterRoute(nameof(Pages.ExplorePage), typeof(Pages.ExplorePage));
            Routing.RegisterRoute(nameof(Pages.EditProfile), typeof(Pages.EditProfile));
            Routing.RegisterRoute(nameof(Pages.NotificationsPage), typeof(Pages.NotificationsPage));
            Routing.RegisterRoute(nameof(Pages.SpaceInformationPage), typeof(Pages.SpaceInformationPage));
            Routing.RegisterRoute(nameof(Pages.SpacePicturesPage), typeof(Pages.SpacePicturesPage));
            Routing.RegisterRoute(nameof(Pages.SpaceTypeSelectionPage), typeof(Pages.SpaceTypeSelectionPage));
            Routing.RegisterRoute(nameof(Pages.CategoryVenuesPage), typeof(Pages.CategoryVenuesPage));

            GoToAsync(nameof(Pages.LoginSignupPage));
        }
    }
}