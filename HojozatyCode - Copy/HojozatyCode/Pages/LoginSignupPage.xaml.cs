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
            // Animate Logo
            await LogoImage.FadeTo(1, 700, Easing.CubicOut);
            await LogoImage.ScaleTo(1, 400, Easing.CubicOut);

            // Animate Subtitle
            await SubtitleLabel.FadeTo(1, 600, Easing.CubicIn);

            // Animate Buttons Stack
            await Task.Delay(200);
            await ButtonStack.FadeTo(1, 600, Easing.CubicIn);
            await ButtonStack.ScaleTo(1, 300, Easing.SpringOut);
        }
    }
}
