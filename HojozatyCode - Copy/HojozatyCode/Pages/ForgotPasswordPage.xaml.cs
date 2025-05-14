using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class ForgotPasswordPage : ContentPage
{
	public ForgotPasswordPage()
	{
		InitializeComponent();
		BindingContext = new ForgotPasswordViewModel();
	}
}