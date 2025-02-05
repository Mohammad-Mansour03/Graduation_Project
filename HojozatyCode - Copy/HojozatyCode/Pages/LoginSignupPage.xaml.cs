using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages
{
    public partial class LoginSignupPage : ContentPage
    {
        public LoginSignupPage()
        {
            InitializeComponent();
            BindingContext = new LoginSignupViewModel();
        }
    }
}