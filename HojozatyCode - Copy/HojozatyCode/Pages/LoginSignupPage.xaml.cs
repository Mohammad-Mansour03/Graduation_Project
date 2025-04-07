using HojozatyCode.ViewModels;
using Microsoft.Maui.Controls;

namespace HojozatyCode.Pages
{
    public partial class LoginSignupPage : ContentPage
    {
        public LoginSignupPage()
        {
            InitializeComponent();
            BindingContext = new LoginSignupViewModel();
            AnimatePage();
        }

        private async void AnimatePage()
        {
            // Logo animation
            await LogoImage.FadeTo(1, 700, Easing.CubicOut);
            await LogoImage.ScaleTo(1, 400, Easing.CubicOut);

            // Subtitle animation
            await SubtitleLabel.FadeTo(1, 600, Easing.CubicIn);

            // Buttons animation
            await Task.Delay(200);
            await ButtonStack.FadeTo(1, 600, Easing.CubicIn);
            await ButtonStack.ScaleTo(1, 300, Easing.SpringOut);
        }
    }
}
