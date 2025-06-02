using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Services;

namespace HojozatyCode.ViewModels;

[QueryProperty(nameof(EmailL), "email")]
public partial class EnterOtpViewModel : ObservableObject
{
	//Store the email that was sent from the previous page
	[ObservableProperty] 
	string emailL;
	
	//store the OTP code the user was sent
	[ObservableProperty] 
	string otpCode;

	//Store the Message to display to the user
	[ObservableProperty]
	string message;

	[RelayCommand]
	private async Task Verify()
	{
		try
		{
			await Shell.Current.DisplayAlert("Prompt", "Inside method navigate me to the Reset Password", "OK");
			var session = await SupabaseConfig.SupabaseClient.Auth.VerifyOTP(
				email: EmailL,
				token: OtpCode,
				type: Supabase.Gotrue.Constants.EmailOtpType.Email
			);

			if (session != null)
			{
				Message = "OTP verified successfully!";

				await Shell.Current.GoToAsync(nameof(Pages.ResetPasswordPage));
			}
			else 
			{
				Message = "OTP verification failed. Please try again.";
				await Shell.Current.GoToAsync(nameof(Pages.ForgotPasswordPage));
			}
		}

		catch (Exception ex)
		{
			Message = $"Verification failed:Token has expired or is invalid.\n{ex.Message}";
		}
	
	}
	

	[RelayCommand]
	private async Task VerifyOtpSignUp()
	{
		try
		{
			await Shell.Current.DisplayAlert("Prompt", "Inside method navigate me to the Profile info Password", "OK");

			var session = await SupabaseConfig.SupabaseClient.Auth.VerifyOTP(
				email: EmailL,
				token: OtpCode,
				type: Supabase.Gotrue.Constants.EmailOtpType.Email
			);

			if (session != null)
			{
				Message = "OTP verified successfully!";

				await Shell.Current.GoToAsync(nameof(Pages.ProfileInfo));
			}
			else 
			{
				Message = "OTP verification failed. Please try again.";
				await Shell.Current.GoToAsync(nameof(Pages.LoginSignupPage));
			}
		}

		catch (Exception ex)
		{
			Message = $"Verification failed:Token has expired or is invalid.\n{ex.Message}";
		}
	
	}
}
