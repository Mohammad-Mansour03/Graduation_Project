using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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
    }
}