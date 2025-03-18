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
    public partial class AdminApprovalViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Venue> pendingVenues;

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

        [RelayCommand]
        private async Task AcceptAsync(Venue venue)
        {
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
            if (venue == null)
            {
                await Shell.Current.DisplayAlert("Error", "Selected venue not found", "OK");
                return;
            }

            try
            {
                bool confirmed = await Shell.Current.DisplayAlert("Confirm", 
                    "Are you sure you want to delete this venue?", "Yes", "No");
                
                if (!confirmed) return;
                
                venue.Status = "Rejected";
                bool success = await VenueService.DeleteVenueAsync(venue);
                if (success)
                {
                    PendingVenues.Remove(venue);
                    await Shell.Current.DisplayAlert("Success", "Venue deleted successfully", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to delete venue", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}