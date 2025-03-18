using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Pages;
using HojozatyCode.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Supabase;

namespace HojozatyCode.ViewModels
{
    public partial class OwnerVenuesViewModel : ObservableObject
    {

        
        [ObservableProperty]
        private ObservableCollection<Venue> ownerVenues;
        
        [ObservableProperty]
        private bool isLoading;
        
        [ObservableProperty]
        private bool isRefreshing;
        
        [ObservableProperty]
        private string errorMessage;
        
        [ObservableProperty]
        private bool hasVenues;

        public OwnerVenuesViewModel()
        {
            OwnerVenues = new ObservableCollection<Venue>();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await LoadOwnerVenues();
        }
        
        [RelayCommand]
        private async Task RefreshVenues()
        {
            IsRefreshing = true;
            await LoadOwnerVenues();
            IsRefreshing = false;
        }
        
        /// <summary>
        /// Loads all venues created by the current user from the database
        /// </summary>
        public async Task LoadOwnerVenues()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                
                // Clear existing venues
                OwnerVenues.Clear();
                
                // Get the current user's ID
                var currentUserId = SupabaseConfig.SupabaseClient.Auth.CurrentUser?.Id;
                
                if (string.IsNullOrEmpty(currentUserId))
                {
                    ErrorMessage = "Please log in to view your venues";
                    HasVenues = false;
                    return;
                }
                
                // Convert string ID to Guid for the query
                var ownerGuid = Guid.Parse(currentUserId);
                
                // Query venues owned by this user
                var response = await SupabaseConfig.SupabaseClient
                    .From<Venue>()
                    .Where(v => v.OwnerId == ownerGuid)
                    .Get();
                
                var venues = response.Models;
                
                // Add venues to the collection
                foreach (var venue in venues)
                {
                    OwnerVenues.Add(venue);
                }
                
                HasVenues = OwnerVenues.Count > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading owner venues: {ex.Message}");
                ErrorMessage = "Failed to load your venues. Please try again later.";
            }
            finally
            {
                IsLoading = false;
            }
        }
        
        // [RelayCommand]
        // private async Task ViewVenueDetails(Venue venue)
        // {
        //     if (venue == null) return;
            
        //     var navigationParameter = new Dictionary<string, object>
        //     {
        //         { "Venue", venue }
        //     };
            
        //     await Shell.Current.GoToAsync(nameof(VenueDetailsPage), navigationParameter);
        // }
        
        // [RelayCommand]
        // private async Task EditVenue(Venue venue)
        // {
        //     if (venue == null) return;
            
        //     var navigationParameter = new Dictionary<string, object>
        //     {
        //         { "Venue", venue }
        //     };
            
        //     await Shell.Current.GoToAsync(nameof(EditVenuePage), navigationParameter);
        // }
        
        // [RelayCommand]
        // private async Task AddNewVenue()
        // {
        //     await Shell.Current.GoToAsync(nameof(AddSpacePage));
        // }
    }
}