namespace HojozatyCode.Pages;

public partial class LogInPage : ContentPage
{
	public LogInPage()
	{
		InitializeComponent();
	}

	private async void SignUp(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Pages.SignUpPage));
    }

	private async void ReturnToThePrevious(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Pages.LoginSignupPage));
	}
}