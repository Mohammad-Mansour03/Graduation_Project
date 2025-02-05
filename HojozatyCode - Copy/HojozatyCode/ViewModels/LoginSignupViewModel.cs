using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace HojozatyCode.ViewModels
{
    public partial class LoginSignupViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task SignUpAsync()
        {
            await Shell.Current.GoToAsync(nameof(Pages.SignUpPage));
        }

        [RelayCommand]
        private async Task LogInAsync()
        {
            await Shell.Current.GoToAsync(nameof(Pages.LogInPage));
        }

        [RelayCommand]
        private async Task ContinueAsGuestAsync()
        {
            await Shell.Current.GoToAsync(nameof(Pages.HomePage));
        }
    }
}