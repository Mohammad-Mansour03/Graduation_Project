namespace HojozatyCode.Pages;

public partial class LoginSignupPage : ContentPage
{
	public LoginSignupPage()
	{
		InitializeComponent();
	}

	private async void SignUp(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Pages.SignUpPage));
    }

	private async void LogIn(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Pages.LogInPage));
	}

	private async void ContinueAsGuest(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Pages.HomePage));
    }
}