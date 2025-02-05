using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;


namespace HojozatyCode.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task NavigateToLogInPageAsync()
        {
            await Shell.Current.GoToAsync(nameof(Pages.LogInPage));
        }

        [RelayCommand]
        private async Task NavigateBackAsync()
        {
            await Shell.Current.GoToAsync(nameof(Pages.LoginSignupPage));
        }
    }
}