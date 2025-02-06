using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

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
            // Add your sign-up logic here
            //await Task.CompletedTask;
            await Shell.Current.GoToAsync(nameof(Pages.ProfileInfo));
        }
    }
}