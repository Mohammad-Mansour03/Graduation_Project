using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Services;
using System.Text.RegularExpressions;

namespace HojozatyCode.ViewModels;

public partial class ResetPasswordViewModel : ObservableObject
{
	//To Sotre the new Password that the user was entered
	[ObservableProperty] 
	string newPassword;
	
	//Sotre the message that will display to the user
	[ObservableProperty]
	string message;

	private  bool IsValidPassword() 
	{
	  return Regex.IsMatch(NewPassword, @"^.{8,}$");
	}


	[RelayCommand]
	public async Task ResetPasswordAsync()
	{
		try
		{
			var user = SupabaseConfig.SupabaseClient.Auth.CurrentUser;

			if (!IsValidPassword()) 
			{
				Message = "Password must be at least 8 characters long";
				return;
			}

			if (user != null)
			{
				
				var result = await SupabaseConfig.SupabaseClient.Auth.Update(new Supabase.Gotrue.UserAttributes
				{
					Password = NewPassword
				});

				if (result != null)
				{
					await Shell.Current.DisplayAlert(" ", "Password updated successfully!","OK");
					await Shell.Current.GoToAsync(nameof(Pages.LogInPage));
				}
				
				else 
				{
					Message = "Your Password Wasn't updated.";
					return;
				}
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
