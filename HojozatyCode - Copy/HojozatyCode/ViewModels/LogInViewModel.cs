using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using System.Threading.Tasks;

namespace HojozatyCode.ViewModels
{
    public partial class LogInViewModel : ObservableObject
    {
		//Properety to Check if the password hidden or not
		[ObservableProperty]
        private bool isHiddenPassword = true;
        
        //Properety to sotre the User email
        [ObservableProperty]
        private string emailL;

		//Properety to sotre the User password
		[ObservableProperty]
        private string passwordL;

        //Property to change eye icon dynamically
        public string EyeIconSource => IsHiddenPassword ? "eye_off_icon.png":"eye_on_icon.png";

		//Properety to soter the error that appear to the user
		[ObservableProperty]
		private string errorMessage;

		//Commant to navigate you to the Sign up Page
		[RelayCommand]
        private async Task SignUpAsync()
        {
            await Shell.Current.GoToAsync(nameof(Pages.SignUpPage));
        }

        //Command to navigate you to the Login Sign up page (For the Previous arrow)
        [RelayCommand]
        private async Task ReturnToThePreviousAsync()
        {
            await Shell.Current.GoToAsync(nameof(Pages.LoginSignupPage));
        }

        //Method to change the password state 
		[RelayCommand]
        private void TogglePasswordVisibility()
        {
            //Toggle visibility
            IsHiddenPassword = !IsHiddenPassword;

            //Notify the UI the Eye icon has changed
            OnPropertyChanged(nameof(EyeIconSource));

        }

		//Command to navigate me to the Forgot Password Page
		[RelayCommand]
		private async Task ForgotPassword()
		{
			await Shell.Current.GoToAsync(nameof(Pages.ForgotPasswordPage));
		}

		private bool IsAdminEmail()
		{
			return string.Equals(EmailL, "admin@hojoz.com") && string.Equals(PasswordL, "admin1234");
		}


		//Make the Login logic for the program
		[RelayCommand]
        private async Task LogInAsync()
        {
			try
			{
				//Check if the Email is null
				if (EmailL == null)
				{
					ErrorMessage = "Please Enter your email";
					return;
				}

				//Check if the Password is null	
				if (PasswordL == null)
				{
					ErrorMessage = "Please Enter your password";
					return;
				}
					
				//Check if the Email and password was for Admin or to User
				if (IsAdminEmail())
				{
					await Shell.Current.GoToAsync(nameof(Pages.AdminPanel));
				}

				else
				{
					// Check if the email was created and authenticated or not
					var response = await SupabaseConfig.SupabaseClient
						.From<User>()
						.Where(x => x.EmailC == EmailL)
						.Get();

					//If the email not found (The email not authenticate yet)
					if (response.Models.FirstOrDefault() == null)
					{
						ErrorMessage = "\t\t\t\t\t\tLogin Failed\nInvalid email or password.";
						return;
					
					}

					else
					{
						//Check if the email and password was matched
						var result = await SupabaseConfig.SupabaseClient.Auth.SignIn(EmailL, PasswordL);
						
						// Login was successful, navigate to the HomePage
						if (result != null)
						{
							await Shell.Current.GoToAsync(nameof(Pages.HomePage));
						}

						else 
						{
							ErrorMessage = "\t\t\t\t\t\tLogin Failed\nInvalid email or password.";
							return;
						}
					}
				}
			}
			catch (Exception ex)
			{
				// Catch other generic exceptions
				await Shell.Current.DisplayAlert("Login Failed", $"Invalid email or password{ex.Message}.", "OK");
			}
		}
    }
}