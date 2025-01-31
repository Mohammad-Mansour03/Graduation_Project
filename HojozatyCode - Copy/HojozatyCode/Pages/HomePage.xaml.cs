namespace HojozatyCode.Pages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

	private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(LogInPage));
	}

	private async void Back(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(LoginSignupPage));
	}
}