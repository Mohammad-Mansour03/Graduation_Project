namespace HojozatyCode.Pages;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
	}

	private async void LogIn(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Pages.LogInPage));
	}

	private async void ReturnToThePrevious(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Pages.LoginSignupPage));
	}
}