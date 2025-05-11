using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class AccountPage : ContentPage
{
	public AccountPage()
	{
		InitializeComponent();
		BindingContext = new AccountPageViewModel();
	}

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }

    private async void OnBorderTapped(object sender, EventArgs e)
    {
        var border = sender as Border;
        if (border == null) return;

        // Animate: quick shrink and bounce back
        await border.ScaleTo(0.95, 80, Easing.CubicIn);
        await border.ScaleTo(1.0, 80, Easing.CubicOut);

        // Execute command (optional)
        if (BindingContext is AccountPageViewModel vm && vm.GoToMySpaceCommand?.CanExecute(null) == true)
        {
            vm.GoToMySpaceCommand.Execute(null);
        }
    }
}