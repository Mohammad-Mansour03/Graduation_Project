using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace HojozatyCode.ViewModels
{
    public partial class ProfileInfoViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync(nameof(Pages.LoginSignupPage));
        }

        [RelayCommand]
        private async Task NextAsync()
        {
            await Shell.Current.GoToAsync(nameof(Pages.HomePage));
        }
    }
}