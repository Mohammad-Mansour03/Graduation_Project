using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using System.Collections.ObjectModel;
using HojozatyCode.Pages;
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

        // Navigate to notification
        [RelayCommand]
        private async Task NavigateToNotifications()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(Pages.NotificationsPage));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Navigation Error", ex.Message, "OK");
                Console.WriteLine($"Navigation Error: {ex.Message}");
            }
        }

    //      [RelayCommand]
    // private async Task NavigateToSpaceTypeSelection()
    // {
    //     await Shell.Current.GoToAsync(nameof(SpaceTypeSelectionPage));
    // }
    }
}