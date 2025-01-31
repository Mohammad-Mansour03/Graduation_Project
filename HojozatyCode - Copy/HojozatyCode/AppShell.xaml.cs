using HojozatyCode.Pages;

namespace HojozatyCode
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Pages.LoginSignupPage),typeof(Pages.LoginSignupPage));
            Routing.RegisterRoute(nameof(Pages.LogInPage),typeof(Pages.LogInPage));
            Routing.RegisterRoute(nameof(Pages.SignUpPage),typeof(Pages.SignUpPage));
            Routing.RegisterRoute(nameof(Pages.ProfileInfo),typeof(Pages.ProfileInfo));
            Routing.RegisterRoute(nameof(Pages.HomePage),typeof(Pages.HomePage));
            Routing.RegisterRoute(nameof(Pages.ChatPage),typeof(Pages.ChatPage));
            Routing.RegisterRoute(nameof(Pages.FavouritePage),typeof(Pages.FavouritePage));
            Routing.RegisterRoute(nameof(Pages.AccountPage),typeof(Pages.AccountPage));
            Routing.RegisterRoute(nameof(Pages.AddSpacePage),typeof(Pages.AddSpacePage));

			GoToAsync(nameof(Pages.LoginSignupPage));


		}

	}
}
