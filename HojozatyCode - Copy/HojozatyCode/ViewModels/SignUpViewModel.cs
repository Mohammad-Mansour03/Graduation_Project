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

        //Properety to soter the error that appear to the user
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

        //Command the have the Sign Up logic and add the user to the database 
        [RelayCommand]
		private async Task SignUpAsync()
        {
            try
            { 
                // Check if email null 
                if (EmailF == null)
                {
                    ErrorMessage = "Please Enter your email";
                    return;
                }

                //Check if the email that user entered has right format
                else
                if (!Regex.IsMatch(EmailF, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z.-]+\.[a-zA-Z]{2,}$"))
                {
                    ErrorMessage = "Please Enter Valid email";
                    return;
                }

                //Check if the Password is null
                else
                if (PasswordF == null)
                {
                    ErrorMessage = "Please enter your password";
                    return;
                }


				// Check password pattern
				else
				if (!Regex.IsMatch(PasswordF, @"^.{8,}$"))
                {
                    ErrorMessage = "Password must be at least 8 characters long";
                    return;
                }

                //Check if the confirm password is null
                if (ConfirmPasswordF == null) 
                {
                    ErrorMessage = "Please enter confirm password";
                    return;
                }

                else 
				// Check if Confirm Password match the Password
				if (!PasswordF.Equals(ConfirmPasswordF))
                {
                    ErrorMessage = "The Confirm Password doesn't match Password";
                    return;
                }
                
                //Store the User in the database (Email and Password)
                var response = await SupabaseConfig.SupabaseClient.Auth.SignUp(EmailF, PasswordF);
                
                //If the user already tored navigate the user to the profile info page
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