using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class AccountPage : ContentPage
{
	public AccountPage()
	{
		InitializeComponent();
		BindingContext = new AccountPageViewModel();
	}


    //For Click Button Design
    private async void OnBorderTapped(object sender, EventArgs e)
    {
        var border = sender as Border;
        if (border == null) return;

        // Animate: quick shrink and bounce back
        await border.ScaleTo(0.95, 80, Easing.CubicIn);
        await border.ScaleTo(1.0, 80, Easing.CubicOut);
         

    }
}