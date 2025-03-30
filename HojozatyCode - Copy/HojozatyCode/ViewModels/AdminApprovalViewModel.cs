using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;

namespace HojozatyCode.ViewModels
{
    [QueryProperty(nameof(SelectedVenueId), "venueId")]
    public partial class AdminApprovalViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Venue> pendingVenues;
        
        [ObservableProperty]
        private Venue selectedVenue;
        
        [ObservableProperty]
        private List<string> venueImageUrls;

        private string selectedVenueId;
        public string SelectedVenueId
        {
            get => selectedVenueId;
            set
            {
                selectedVenueId = value;
                if (!string.IsNullOrEmpty(value))
                {
                    LoadSelectedVenue(Guid.Parse(value));
                }
            }
        }

        public AdminApprovalViewModel()
        {
            PendingVenues = new ObservableCollection<Venue>();
            LoadPendingVenues();
        }

        private async void LoadPendingVenues()
        {
            try
            {
                var venues = await VenueService.GetPendingVenuesAsync();
                foreach (var venue in venues.Where(v => v != null))
                {
                    PendingVenues.Add(venue);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading pending venues: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to load venues", "OK");
            }
        }
        
        private async void LoadSelectedVenue(Guid venueId)
        {
            try
            {
                // First check if it's in our already loaded collection
                var venue = PendingVenues.FirstOrDefault(v => v.VenueId == venueId);
                
                if (venue == null)
                {
                    // If not found in the collection, fetch it directly
                    var response = await SupabaseConfig.SupabaseClient
                        .From<Venue>()
                        .Where(v => v.VenueId == venueId)
                        .Get();
                        
                    if (response != null && response.Models.Count > 0)
                    {
                        venue = response.Models[0];
                    }
                }
                
                if (venue != null)
                {
                    SelectedVenue = venue;
                    
                    // Use identical code from AddSpaceViewModel for consistency
                    if (!string.IsNullOrEmpty(venue.ImageUrl))
                    {
                        VenueImageUrls = venue.ImageUrl
                            .Split(',')
                            .Where(url => !string.IsNullOrWhiteSpace(url))
                            .ToList();
                        
                        // Force property change notification
                        OnPropertyChanged(nameof(VenueImageUrls));
                        
                        // Debug output
                        Debug.WriteLine($"Found {VenueImageUrls.Count} image URLs");
                        foreach (var url in VenueImageUrls)
                        {
                            Debug.WriteLine($"Image URL: {url}");
                        }
                    }
                    else
                    {
                        VenueImageUrls = new List<string>();
                        Debug.WriteLine("No image URLs found");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading selected venue: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to load venue details", "OK");
            }
        }

        [RelayCommand]
        private async Task AcceptAsync(Venue venue)
        {
            // Use either the passed venue or the selected venue
            venue = venue ?? SelectedVenue;
            
            try
            {
                if (venue == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Venue is null. Command parameter not properly passed.", "OK");
                    return;
                }

                Console.WriteLine($"AcceptAsync received venue: {venue.VenueName}, ID: {venue.VenueId}");
                venue.Status = "Approved";
                bool success = await VenueService.UpdateVenueStatusAsync(venue);
                
                if (success)
                {
                    PendingVenues.Remove(venue);
                    await Shell.Current.DisplayAlert("Success", "Venue approved successfully", "OK");
                    await GoBack();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to approve venue", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AcceptAsync: {ex}");
                await Shell.Current.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task DeleteAsync(Venue venue)
        {
            // Use either the passed venue or the selected venue
            venue = venue ?? SelectedVenue;
            
            if (venue == null)
            {
                await Shell.Current.DisplayAlert("Error", "Selected venue not found", "OK");
                return;
            }

            try
            {
                bool confirmed = await Shell.Current.DisplayAlert("Confirm", 
                    "Are you sure you want to reject this venue?", "Yes", "No");
                
                if (!confirmed) return;
                
                venue.Status = "Rejected";
                bool success = await VenueService.DeleteVenueAsync(venue);
                if (success)
                {
                    PendingVenues.Remove(venue);
                    await Shell.Current.DisplayAlert("Success", "Venue rejected successfully", "OK");
                    await GoBack();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to reject venue", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
        
        [RelayCommand]
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}