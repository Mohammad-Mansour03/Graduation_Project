namespace HojozatyCode.Pages;

public partial class SignUpPage : ContentPage
{
	bool isVisiblePassword = false;
	bool isVisibleConfirmPassword = false;
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

	private void OnEyeIconClicked_Password(object sender, EventArgs e)
	{
		isVisiblePassword = !isVisiblePassword;

		PasswordEntry.IsPassword = !isVisiblePassword;

		EyeIconButton.Source = isVisiblePassword ? "eye_on_icon.png" : "eye_off_icon.png";
    }
	private void OnEyeIconClickedConfirmPass(object sender, EventArgs e)
	{
		isVisibleConfirmPassword = !isVisibleConfirmPassword;

		ConfirmPasswordEntry.IsPassword = !isVisibleConfirmPassword;

		EyeIconButtonConfirm.Source = isVisibleConfirmPassword ? "eye_on_icon.png" : "eye_off_icon.png";
    }

}