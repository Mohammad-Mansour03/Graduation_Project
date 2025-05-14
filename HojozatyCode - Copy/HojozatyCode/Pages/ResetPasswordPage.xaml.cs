using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class ResetPasswordPage : ContentPage
{
	public ResetPasswordPage()
	{
		InitializeComponent();
		BindingContext = new ResetPasswordViewModel();
	}
}