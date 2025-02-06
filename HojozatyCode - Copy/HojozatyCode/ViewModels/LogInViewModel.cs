using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace HojozatyCode.ViewModels
{
    public partial class LogInViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isHiddenPassword = true;

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
            // Add your login logic here
            //await Task.CompletedTask;
            await Shell.Current.GoToAsync(nameof(Pages.HomePage));
        }
    }
}