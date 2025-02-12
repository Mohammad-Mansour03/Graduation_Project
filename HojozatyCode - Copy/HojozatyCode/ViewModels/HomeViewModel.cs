using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using System.Collections.ObjectModel;

namespace HojozatyCode.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Venue> topVenues;

        public HomeViewModel()
        {
            LoadTopVenues();
        }

        private async void LoadTopVenues()
        {
            try
            {
                var response = await SupabaseConfig.SupabaseClient
                    .From<Venue>()
                    .Get();

                if (response != null && response.Models != null)
                {
                    TopVenues = new ObservableCollection<Venue>(response.Models);
                    await Shell.Current.DisplayAlert("Success", $"Fetched {response.Models.Count} venues", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "No venues found", "OK");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                await Shell.Current.DisplayAlert("Error", $"Error fetching venues: {ex.Message}", "OK");
                Console.WriteLine($"Error fetching venues: {ex.Message}");
            }
        }

        // Navigation Command
        [RelayCommand]
        private async Task NavigateToExplore()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(Pages.ExplorePage));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Navigation Error", ex.Message, "OK");
                Console.WriteLine($"Navigation Error: {ex.Message}");
            }
        }

		// Navigate to the profile page
		[RelayCommand]
		private async Task NavigateToProfile()
		{
			try
			{
				await Shell.Current.GoToAsync(nameof(Pages.ProfilePage));
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Navigation Error", ex.Message, "OK");
				Console.WriteLine($"Navigation Error: {ex.Message}");
			}
		}

		// Navigate to the profile page
		[RelayCommand]
		private async Task NavigateBackCommand()
		{
			try
			{
				await Shell.Current.GoToAsync(nameof(Pages.LoginSignupPage));
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Navigation Error", ex.Message, "OK");
				Console.WriteLine($"Navigation Error: {ex.Message}");
			}
		}
	}
}
