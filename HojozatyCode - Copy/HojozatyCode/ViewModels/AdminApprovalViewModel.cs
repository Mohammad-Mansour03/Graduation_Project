using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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
            var venues = await VenueService.GetPendingVenuesAsync();
            foreach (var venue in venues)
            {
                PendingVenues.Add(venue);
            }
        }

        //[RelayCommand]
        // private async Task AcceptAsync(Venue venue)
        // {
        //     if (venue == null)
        //     {
        //         Console.WriteLine("AcceptAsync: Venue is null.");
        //         return;
        //     }

        //     venue.Status = "Approved";
        //     bool success = await VenueService.UpdateVenueStatusAsync(venue);
        //     if (success)
        //     {
        //         PendingVenues.Remove(venue);
        //     }
        //     else
        //     {
        //         // Handle failure (e.g., show an error message)
        //         Console.WriteLine("AcceptAsync: Failed to update venue status.");
        //     }
        // }

        // [RelayCommand]
        // private async Task DeleteAsync(Venue venue)
        // {
        //     if (venue == null)
        //     {
        //         Console.WriteLine("DeleteAsync: Venue is null.");
        //         return;
        //     }

        //     bool success = await VenueService.DeleteVenueAsync(venue);
        //     if (success)
        //     {
        //         PendingVenues.Remove(venue);
        //     }
        //     else
        //     {
        //         // Handle failure (e.g., show an error message)
        //         Console.WriteLine("DeleteAsync: Failed to delete venue.");
        //     }
        // }
    }
}