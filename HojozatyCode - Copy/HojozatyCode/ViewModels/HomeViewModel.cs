using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using System.Collections.ObjectModel;

namespace HojozatyCode.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync(nameof(Pages.LoginSignupPage));
        }     
        
        [RelayCommand]
        private async Task GoToExplore()
        {
            await Shell.Current.GoToAsync(nameof(Pages.ExplorePage));
        }
        
        [RelayCommand]
        private async Task GoToAccount()
        {
            await Shell.Current.GoToAsync(nameof(Pages.AccountPage));
        }
    }
}