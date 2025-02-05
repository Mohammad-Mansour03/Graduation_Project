using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace HojozatyCode.ViewModels
{
    public partial class SignUpViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isVisiblePassword = false;

        [ObservableProperty]
        private bool isVisibleConfirmPassword = false;

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
            IsVisiblePassword = !IsVisiblePassword;
        }

        [RelayCommand]
        private void ToggleConfirmPasswordVisibility()
        {
            IsVisibleConfirmPassword = !IsVisibleConfirmPassword;
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