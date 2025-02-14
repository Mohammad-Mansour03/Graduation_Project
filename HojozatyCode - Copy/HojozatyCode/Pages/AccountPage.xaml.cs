using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class AccountPage : ContentPage
{
	public AccountPage()
	{
		InitializeComponent();
		BindingContext = new AccountPageViewModel();
	}
}