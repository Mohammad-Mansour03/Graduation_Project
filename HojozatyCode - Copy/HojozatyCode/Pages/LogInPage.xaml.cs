using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages
{
    public partial class LogInPage : ContentPage
    {
        public LogInPage()
        {
            InitializeComponent();
            BindingContext = new LogInViewModel();
        }

        //For Front-End Design
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Fade-in animation
            this.Opacity = 0;
            await this.FadeTo(1, 800, Easing.CubicInOut);

            // Slide in Back Button
            BackButton.TranslationX = -100;
            await BackButton.TranslateTo(0, 0, 500, Easing.CubicOut);
        }
    }
}
