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

        }
        
    }
}
