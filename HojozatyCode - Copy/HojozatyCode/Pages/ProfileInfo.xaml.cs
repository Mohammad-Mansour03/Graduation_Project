namespace HojozatyCode.Pages;


public partial class ProfileInfo : ContentPage
{
    public ProfileInfo()
    {
        InitializeComponent();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
      await Shell.Current.GoToAsync(nameof(LoginSignupPage));
    }

    private async void OnNextClicked(object sender, EventArgs e)
    {
      await Shell.Current.GoToAsync(nameof(Pages.HomePage));
      
    }
}
