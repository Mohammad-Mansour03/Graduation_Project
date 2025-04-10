using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using System.Collections.ObjectModel;

namespace HojozatyCode.ViewModels
{
    [QueryProperty("Category", "Category")]
    public partial class CategoryVenuesViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<string> subCategories = new List<string>();

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
            var spaceCategories = new Dictionary<string, List<string>>
            {
                { "Wedding", new List<string> { "Halls", "Farms", "Hotels", "Outdoors" } },
                { "Entertainment", new List<string> { "Farms", "Adventure Spots", "WorkShops" } },
                { "Work/Meeting Space", new List<string> { "ClassRooms/Office Spaces", "Farms", "Outdoor Space", "Majls" } },
                { "Funeral", new List<string> { "Diwan", "Dedicated Funeral Halls" } },
                { "Photography", new List<string> { "Photography Studios", "Outdoor Photography Spaces", "Product Photography Spaces" } },
                { "Sports", new List<string> { "Staduim" } },
                { "Cultural Events", new List<string> { "Farms", "Majls", "Cultrual Evening Venues", "Theaters and Cultural halls" } }
            };

            try
            {
                // Update SubCategories using the ObservableProperty pattern
                SubCategories = spaceCategories.ContainsKey(Category) ? spaceCategories[Category] : new List<string>();

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