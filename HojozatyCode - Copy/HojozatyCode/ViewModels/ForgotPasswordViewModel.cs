using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Services;

namespace HojozatyCode.ViewModels;

public partial class ForgotPasswordViewModel : ObservableObject
{
	[ObservableProperty] string emailL;
	[ObservableProperty] string message;

	[RelayCommand]
	public async Task SendOtpAsync()
	{
		try
		{
			var respons = await SupabaseConfig.SupabaseClient.Auth.ResetPasswordForEmail(EmailL);

			if (respons == true)
			{
				Message = "OTP has been sent to your email.";
				await Shell.Current.GoToAsync($"{nameof(Pages.EnterOTPPage)}?email={EmailL}");

			}
			else 
			{
				Message = "Failed To Send it";
				return;
			}
			// Navigate to EnterOtpPage with email
		}
		catch (Exception ex)
		{
			Message = $"Error: {ex.Message}";
		}
	}
}
