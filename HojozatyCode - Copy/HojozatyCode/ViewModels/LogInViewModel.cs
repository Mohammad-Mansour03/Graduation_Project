using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace HojozatyCode.ViewModels
{
    public partial class LogInViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isVisiblePassword = false;

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
            IsVisiblePassword = !IsVisiblePassword;
        }

        [RelayCommand]
        private async Task LogInAsync()
        {
            // Add your login logic here
            //await Task.CompletedTask;
            await Shell.Current.GoToAsync(nameof(Pages.HomePage));
        }
    }
}