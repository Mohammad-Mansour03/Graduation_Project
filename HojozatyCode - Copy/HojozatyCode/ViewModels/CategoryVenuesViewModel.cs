using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using System.Collections.ObjectModel;

namespace HojozatyCode.ViewModels
{



    [QueryProperty("Category", "Category")]
    [QueryProperty("Location", "Location")]
    public partial class CategoryVenuesViewModel : ObservableObject
    {

        [RelayCommand]
        private async Task GoToFiltersPage()
        {
            await Shell.Current.GoToAsync(nameof(Pages.FiltersPage));
        }

        [ObservableProperty]
        private string category;

        [ObservableProperty]
        private string location;

        [ObservableProperty]
        private ObservableCollection<Venue> venues;

        public CategoryVenuesViewModel()
        {
            Venues = new ObservableCollection<Venue>();
        }

        // Change this to partial method
        partial void OnCategoryChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Task.Run(async () => await LoadVenuesForCategory(value));
            }
        }

        private async Task LoadVenuesForCategory(string categoryName)
        {
            try
            {
                var client = SupabaseConfig.SupabaseClient;
                var venuesResult = await client
                    .From<Venue>()
                    .Where(v => v.Type == categoryName)
                    .Get();

                Venues.Clear();
                foreach (var venue in venuesResult.Models)
                {
                    Venues.Add(venue);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load venues: {ex.Message}", "OK");
            }
        }
    }
}