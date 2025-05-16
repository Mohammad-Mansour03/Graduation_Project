using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;

namespace HojozatyCode.ViewModels;

public partial class ForgotPasswordViewModel : ObservableObject
{
	//Propert to sotre the email data to recover the passowrd
	[ObservableProperty] 
	string emailL;

	//Message to display to the user 
	[ObservableProperty] 
	string message;

	private async Task<bool> IsEmailFound() 
	{
		var response = await SupabaseConfig.SupabaseClient
		.From<User>()
		.Where(x => x.EmailC == EmailL)
		.Get();


		if (response.Models.FirstOrDefault() != null)
			return true;

		return false;
	}

	[RelayCommand]
	public async Task SendOtpAsync()
	{
		try
		{
			
			if (!await IsEmailFound())
			{
				Message = "Email Wasn't found";
				return;
			}

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
