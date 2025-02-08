using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Services;
using System.Threading.Tasks;

namespace HojozatyCode.ViewModels
{
    public partial class LogInViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isHiddenPassword = true;
        
        [ObservableProperty]
        private string emailL;
        
        [ObservableProperty]
        private string passwordL;

        //Property to change eye icon dynamically
       public string EyeIconSource => IsHiddenPassword ? "eye_off_icon.png":"eye_on_icon.png";

        [RelayCommand]
        private async Task SignUpAsync()
        {
            await Shell.Current.GoToAsync(nameof(Pages.SignUpPage));
        }

        [RelayCommand]
        private async Task ReturnToThePreviousAsync()
        {
            await Shell.Current.GoToAsync(nameof(Pages.LoginSignupPage));
        }

        [RelayCommand]
        private void TogglePasswordVisibility()
        {
            //Toggle visibility
            IsHiddenPassword = !IsHiddenPassword;

            //Notify the UI the Eye icon has changed
            OnPropertyChanged(nameof(EyeIconSource));

        }

        [RelayCommand]
        private async Task LogInAsync()
        {
			try
			{
				// Try signing in with email and password
				var response = await SupabaseConfig.SupabaseClient.Auth.SignIn(EmailL, PasswordL);

				// Check if the login was successful
				if (response.User != null)
				{
					// Login was successful, navigate to the HomePage
					await Shell.Current.GoToAsync(nameof(Pages.HomePage));
				}
				else
				{
					// Show an error message if login fails (e.g., invalid credentials)
					await Shell.Current.DisplayAlert("Login Failed", "Invalid email or password.", "OK");
				}
			}
		
			catch (Exception ex)
			{
				// Catch other generic exceptions
				await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
			}
		}
    }
}