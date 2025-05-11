using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using Microsoft.Maui.Dispatching;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HojozatyCode.ViewModels
{    public partial class FavoriteViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Venue> favoriteVenues = new();

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool isEmpty;
        
        [ObservableProperty]
        private string errorMessage;

        // Flags to prevent multiple loads
        private bool _isInitialized = false;
        private bool _isLoadingData = false;

        public FavoriteViewModel()
        {
            // Only queue this once
            if (!_isInitialized)
            {
                MainThread.BeginInvokeOnMainThread(async () => 
                {
                    await InitializeAsync();
                });
            }
        }

        private async Task InitializeAsync()
        {
            if (_isInitialized) return;
            
            IsLoading = true;
            await LoadFavoritesAsync();
            IsLoading = false;
            
            _isInitialized = true;
        }        [RelayCommand]
        public async Task RefreshFavorites()
        {
            // Prevent refresh if already loading
            if (IsLoading || _isLoadingData) 
            {
                Debug.WriteLine("RefreshFavorites - Already loading data, ignoring this refresh");
                return;
            }
            
            IsLoading = true;
            await LoadFavoritesAsync();
            IsLoading = false;
        }public async Task LoadFavoritesAsync()
        {
            // Prevent concurrent loads by checking the _isLoadingData flag
            if (_isLoadingData) 
            {
                Debug.WriteLine("LoadFavoritesAsync - Already loading data, skipping this call");
                return;
            }
            
            _isLoadingData = true;
            
            try
            {
                if (SupabaseConfig.SupabaseClient == null)
                    await SupabaseConfig.InitializeAsync();

                ErrorMessage = string.Empty;

                // Get current user ID
                var session = SupabaseConfig.SupabaseClient.Auth.CurrentSession;
                if (session == null || session.User == null)
                {
                    ErrorMessage = "No user is logged in";
                    IsEmpty = true;
                    return;
                }

                var userId = Guid.Parse(session.User.Id);

                // Clear current favorites
                FavoriteVenues.Clear();

                // First, get all favorite venue IDs for the current user
                var userFavorites = await SupabaseConfig.SupabaseClient
                    .From<UserFavorite>()
                    .Where(f => f.UserId == userId)
                    .Get();

                if (userFavorites == null || userFavorites.Models.Count == 0)
                {
                    IsEmpty = true;
                    return;
                }

                // Extract UNIQUE venue IDs from user favorites
                var venueIds = userFavorites.Models
                    .Select(f => f.VenueId)
                    .Distinct()
                    .ToList();

                // Create a HashSet to track venues we've already added
                var addedVenueIds = new HashSet<Guid>();

                // Fetch venue details for each venue ID
                foreach (var venueId in venueIds)
                {
                    // Skip if we've already added this venue
                    if (addedVenueIds.Contains(venueId))
                        continue;
                        
                    var venueResponse = await SupabaseConfig.SupabaseClient
                        .From<Venue>()
                        .Where(v => v.VenueId == venueId)
                        .Get();

                    if (venueResponse != null && venueResponse.Models.Count > 0)
                    {
                        FavoriteVenues.Add(venueResponse.Models[0]);
                        addedVenueIds.Add(venueId);
                    }
                }

                IsEmpty = FavoriteVenues.Count == 0;
            }            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading favorites: {ex.Message}");
                ErrorMessage = "Failed to load favorites. Please try again.";
            }
            finally
            {
                // Always release the lock
                _isLoadingData = false;
                Debug.WriteLine("LoadFavoritesAsync - Finished loading data");
            }
        }

        [RelayCommand]
        public async Task VenueSelectedAsync(Venue selectedVenue)
        {
            if (selectedVenue == null) return;

            var navParams = new Dictionary<string, object>
            {
                { "SelectedVenue", selectedVenue }
            };
            
            await Shell.Current.GoToAsync(nameof(Pages.ChoosingHallBooking), navParams);
        }

        [RelayCommand]
        public async Task RemoveFavorite(Venue venue)
        {
            if (venue == null) return;

            try
            {
                var session = SupabaseConfig.SupabaseClient.Auth.CurrentSession;
                if (session == null || session.User == null)
                {
                    await Shell.Current.DisplayAlert("Error", "No user is logged in", "OK");
                    return;
                }

                var userId = Guid.Parse(session.User.Id);

                // Delete the favorite from the database
                await SupabaseConfig.SupabaseClient
                    .From<UserFavorite>()
                    .Where(f => f.UserId == userId && f.VenueId == venue.VenueId)
                    .Delete();

                // Remove from the local collection
                FavoriteVenues.Remove(venue);
                IsEmpty = FavoriteVenues.Count == 0;

                await Shell.Current.DisplayAlert("Success", "Venue removed from favorites", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error removing favorite: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to remove venue from favorites", "OK");
            }
        }
    }
}