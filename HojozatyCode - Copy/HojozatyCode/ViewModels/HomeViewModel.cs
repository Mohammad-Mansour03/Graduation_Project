using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using System.Collections.ObjectModel;
using HojozatyCode.Pages;
namespace HojozatyCode.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        //Command for back icon (Navigate me to the Login and Signup page)
        [RelayCommand]
        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync(nameof(Pages.LoginSignupPage));
        }

        //Navigate me to the Explore page
        [RelayCommand]
        private async Task GoToExplore()
        {
            await Shell.Current.GoToAsync(nameof(Pages.ExplorePage));
        }

        //Navigate me to the Account Page
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
                return;
            }
        }
       
        [RelayCommand]
        private async Task NavigateToCategory(string category)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "Category", category }
                };
                await Shell.Current.GoToAsync($"{nameof(CategoryVenuesPage)}", true, parameters);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Navigation Error", ex.Message, "OK");
                Console.WriteLine($"Navigation Error: {ex.Message}");
            }
        }

         [RelayCommand]
        private async Task NavigateToAdminApprovalAsync()
        {
            await Shell.Current.GoToAsync(nameof(Pages.AdminApprovalPage));
        }
    }
}
