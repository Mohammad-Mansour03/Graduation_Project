using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class EnterOTPSignUp : ContentPage
{
	public EnterOTPSignUp()
	{
		InitializeComponent();
		BindingContext = new EnterOtpViewModel();
	}
}