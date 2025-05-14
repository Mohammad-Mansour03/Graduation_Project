using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Services;

namespace HojozatyCode.ViewModels;

[QueryProperty(nameof(EmailL), "email")]
public partial class EnterOtpViewModel : ObservableObject
{
	[ObservableProperty] string emailL;
	[ObservableProperty] string otpCode;
	[ObservableProperty] string message;

	[RelayCommand]
	public async Task VerifyOtpAsync()
	{
		try
		{
			var session = await SupabaseConfig.SupabaseClient.Auth.VerifyOTP(
				email: EmailL,
				token: OtpCode,
				type: Supabase.Gotrue.Constants.EmailOtpType.Email
			);

			Message = "OTP verified!";

			// Navigate to ResetPasswordPage
			await Shell.Current.GoToAsync(nameof(Pages.ResetPasswordPage));
		}
		catch (Exception ex)
		{
			Message = $"Verification failed: {ex.Message}";
		}
	}
}
