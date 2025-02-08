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

        [ObservableProperty]
        private bool isHiddenPassword = true;

        [ObservableProperty]
        private bool isHiddenConfirmPassword = true;

        public string EyeIconPasswordSource => IsHiddenPassword ? "eye_off_icon.png" : "eye_on_icon.png";
    
        public string EyeIconConfirmPasswordSource => IsHiddenConfirmPassword ? "eye_off_icon.png" : "eye_on_icon.png";

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private string errorMessage;


        [ObservableProperty]
        private string emailF;

        [ObservableProperty]
        private string passwordF;
        
        [ObservableProperty]
        private string confirmPasswordF;

		[RelayCommand]
        private async Task LogInAsync()
        {
            await Shell.Current.GoToAsync(nameof(Pages.LogInPage));
        }

        [RelayCommand]
        private async Task ReturnToThePreviousAsync()
        {
            await Shell.Current.GoToAsync(nameof(Pages.LoginSignupPage));
        }

        [RelayCommand]
        private void TogglePasswordVisibility()
        {
            //Toggle Visibility
            IsHiddenPassword = !IsHiddenPassword;

            //Notfiy the UI that the eye password changed
            OnPropertyChanged(nameof(EyeIconPasswordSource));
        }

        [RelayCommand]
        private void ToggleConfirmPasswordVisibility()
        {
            //Toggle Visibility
            IsHiddenConfirmPassword = !IsHiddenConfirmPassword;

            //Notify that the eye confirm password changed
            OnPropertyChanged(nameof(EyeIconConfirmPasswordSource));
        }

        [RelayCommand]
		private async Task SignUpAsync()
        {
            try
            { 
                // Check email 
                if (EmailF == null)
                {
                    ErrorMessage = "Please Enter your email";
                    return;
                }

                else
                if (!Regex.IsMatch(EmailF, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    ErrorMessage = "Please Enter Valid email";
                    return;
                }

                else
                if (PasswordF == null)
                {
                    ErrorMessage = "Please enter your password";
                    return;
                }

                else
                // Check password pattern
                if (!Regex.IsMatch(PasswordF, @"^.{8,}$"))
                {
                    ErrorMessage = "Password must be at least 8 characters long";
                    return;
                }

                if (ConfirmPasswordF == null) 
                {
                    ErrorMessage = "Please enter confirm password";
                    return;
                }

                else 
				// Check if passwords match
				if (!PasswordF.Equals(ConfirmPasswordF))
                {
                    ErrorMessage = "Passwords doesn't not match";
                    return;
                }
                
                var response = await SupabaseConfig.SupabaseClient.Auth.SignUp(EmailF, PasswordF);
             
                if (response.User != null)
                {
                    var session = await SupabaseConfig.SupabaseClient.Auth.SignIn(EmailF, PasswordF);

                    if (session != null)
                    {
                        await Shell.Current.GoToAsync(nameof(Pages.ProfileInfo));
                    }
                }
                else
                {
                    // Handle error
                    await Shell.Current.DisplayAlert("Sign Up Failed", 
                        "An error occurred during sign up. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                await Shell.Current.DisplayAlert("Sign Up Failed", ex.Message, "OK");
            }
        }
    }
}