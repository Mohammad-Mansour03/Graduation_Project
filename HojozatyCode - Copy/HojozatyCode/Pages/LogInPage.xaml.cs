namespace HojozatyCode.Pages;

public partial class LogInPage : ContentPage
{
	bool isVisiblePassword = false;

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

	private void EyeIconButton_Clicked(object sender, EventArgs e)
	{
		isVisiblePassword = !isVisiblePassword;

		PasswordEntry.IsPassword = !isVisiblePassword;

		EyeIconButton.Source = isVisiblePassword ? "eye_on_icon.png" : "eye_off_icon.png";
	}

}