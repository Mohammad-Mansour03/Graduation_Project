using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class EnterOTPPage : ContentPage
{
	public EnterOTPPage()
	{
		InitializeComponent();
		BindingContext = new EnterOtpViewModel();
	}
}