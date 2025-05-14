using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Services;

namespace HojozatyCode.ViewModels;

public partial class ResetPasswordViewModel : ObservableObject
{
	[ObservableProperty] string newPassword;
	[ObservableProperty] string message;

	[RelayCommand]
	public async Task ResetPasswordAsync()
	{
		try
		{
			var user = SupabaseConfig.SupabaseClient.Auth.CurrentUser;
			if (user != null)
			{
				await SupabaseConfig.SupabaseClient.Auth.Update(new Supabase.Gotrue.UserAttributes
				{
					Password = NewPassword
				});

				Message = "Password updated successfully!";
				// Optionally navigate to LoginPage
			}
			else
			{
				Message = "Session expired. Please try again.";
			}
		}
		catch (Exception ex)
		{
			Message = $"Error: {ex.Message}";
		}
	}
}
