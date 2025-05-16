using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace HojozatyCode.ViewModels
{
	public partial class SignUpViewModel : ObservableObject
	{

		//Properety to Check if the password hidden or not
		[ObservableProperty]
		private bool isHiddenPassword = true;


		//Properety to Check if the confirm password hidden or not
		[ObservableProperty]
		private bool isHiddenConfirmPassword = true;


		//Properety to Check what the eye icon appear
		public string EyeIconPasswordSource => IsHiddenPassword ? "eye_off_icon.png" : "eye_on_icon.png";


		//Properety to Check what the eye icon appear
		public string EyeIconConfirmPasswordSource => IsHiddenConfirmPassword ? "eye_off_icon.png" : "eye_on_icon.png";

		//Properety to store the error that appear to the user
		[ObservableProperty]
		private string errorMessage;

		//Properety to store the email that user entered
		[ObservableProperty]
		private string emailF;

		//Properety to store the password that user entered
		[ObservableProperty]
		private string passwordF;


		//Properety to Store the confirm password that user entered
		[ObservableProperty]
		private string confirmPasswordF;

		//Command to navigate you to the log in page
		[RelayCommand]
		private async Task LogInAsync()
		{
			await Shell.Current.GoToAsync(nameof(Pages.LogInPage));
		}

		//Command to navigate you to the log in sign up page (By the previous arrow)
		[RelayCommand]
		private async Task ReturnToThePreviousAsync()
		{
			await Shell.Current.GoToAsync(nameof(Pages.LoginSignupPage));
		}

		//Method to change the password state 
		[RelayCommand]
		private void TogglePasswordVisibility()
		{
			//Toggle Visibility
			IsHiddenPassword = !IsHiddenPassword;

			//Notfiy the UI that the eye password changed
			OnPropertyChanged(nameof(EyeIconPasswordSource));
		}


		//Method to change the password state 
		[RelayCommand]
		private void ToggleConfirmPasswordVisibility()
		{
			//Toggle Visibility
			IsHiddenConfirmPassword = !IsHiddenConfirmPassword;

			//Notify that the eye confirm password changed
			OnPropertyChanged(nameof(EyeIconConfirmPasswordSource));
		}

		//Method to Validate the email if it was correct or not
		private bool ValidateEmail(string email) 
		{
			return Regex.IsMatch(EmailF, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z.-]+\.[a-zA-Z]{2,}$");
		}
			
		//Method to Validate the Password if it was correct or not
		private bool ValidatePassword(string password) 
		{
			return Regex.IsMatch(PasswordF, @"^.{8,}$"); 
		}




		[RelayCommand]
		private async Task SignUpAsync()
		{
			try
			{
				ErrorMessage = string.Empty;

				// Validate if the user enter the email entry
				if (string.IsNullOrWhiteSpace(EmailF))
				{
					ErrorMessage = "Please enter your email";
					return;
				}

				//Check if the email has a correct d
				if (!ValidateEmail(EmailF))
				{
					ErrorMessage = "Please enter a valid email";
					return;
				}

				// Validate if the user enter the password entry
				if (string.IsNullOrWhiteSpace(PasswordF))
				{
					ErrorMessage = "Please enter your password";
					return;
				}

				if (!ValidatePassword(PasswordF))
				{
					ErrorMessage = "Password must be at least 8 characters long";
					return;
				}

				// Confirm password check
				if (string.IsNullOrWhiteSpace(ConfirmPasswordF))
				{
					ErrorMessage = "Please enter confirm password";
					return;
				}

				if (!PasswordF.Equals(ConfirmPasswordF))
				{
					ErrorMessage = "The confirm password doesn't match the password";
					return;
				}


				//Check if the email was already exist or not
				var result = await SupabaseConfig.SupabaseClient
					.From<User>()
					.Where(x => x.EmailC == EmailF)
					.Get();

				if (result.Models.FirstOrDefault() != null) 
				{
					ErrorMessage = "The Email Already Signed Up Before";
					return;
				}

				// Try to sign up
				var response = await SupabaseConfig.SupabaseClient.Auth.SignUp(EmailF, PasswordF);

				if (response.User != null)
				{
					if (!response.User.ConfirmedAt.HasValue)
					{
						await Shell.Current.DisplayAlert("Confirmation Required",
							"A confirmation email has been sent. Please check your inbox to verify your email.",
							"OK");

						// Poll every 3 seconds to check if user confirmed their email
						for (int i = 0; i < 20; i++) // 20 attempts = ~1 minute
						{
							await Task.Delay(11000); // Wait 11 seconds

							var session = await SupabaseConfig.SupabaseClient.Auth.SignIn(EmailF, PasswordF);

							if (session != null && session.User?.ConfirmedAt != null)
							{
								await Shell.Current.GoToAsync(nameof(Pages.ProfileInfo));
								return;
							}
						}

						await Shell.Current.DisplayAlert("Still Not Confirmed",
							"Email not confirmed yet. Please confirm your email before continuing.", "OK");
					}
					else
					{
						await Shell.Current.GoToAsync(nameof(Pages.ProfileInfo));
					}
				}
				else
				{
					await Shell.Current.DisplayAlert("Sign Up Failed",
						"An unknown error occurred during sign up. Please try again.", "OK");
				}
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Sign Up Failed", ex.Message, "OK");
			}
		}


	}
}