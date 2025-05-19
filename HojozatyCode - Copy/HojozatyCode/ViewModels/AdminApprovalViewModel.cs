using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;

namespace HojozatyCode.ViewModels
{
    [QueryProperty(nameof(SelectedVenueId), "venueId")]
    public partial class AdminApprovalViewModel : ObservableObject
    {
        //Collection to store all Pending Venues
        [ObservableProperty]
        private ObservableCollection<Venue> pendingVenues;

        [ObservableProperty]
        private int numberOfApprovedVenues;

		//Properety to store the Venue that the admin selected
		[ObservableProperty]
        private Venue selectedVenue;
        
        //Collection to store the all venue images
        [ObservableProperty]
        private ObservableCollection<string> venueImageUrls = new();

        //Property to store the Passed venue id
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

        //Command to log out the Admin
        [RelayCommand]
        private async Task Logout()
        {
            try
            {
                await SupabaseConfig.SupabaseClient.Auth.SignOut();

                await Shell.Current.GoToAsync(nameof(Pages.LogInPage));

            }
			catch (Exception ex)
			{
				// Optional: show error to user
				await Shell.Current.DisplayAlert("Logout Failed", ex.Message, "OK");
			}

		}


        public AdminApprovalViewModel()
        {
            PendingVenues = new ObservableCollection<Venue>();
        }

      //Command to Load all venues with Pending Status 
        public async void LoadPendingVenues()
        {
            try
            {
                PendingVenues.Clear();

                var venues = await VenueService.GetPendingVenuesAsync();

                var allVenues = await SupabaseConfig.SupabaseClient
                    .From<Venue>()
                    .Where(x => x.Status == "Approved")
                    .Get();

                NumberOfApprovedVenues = allVenues.Models.Count();

                foreach (var venue in venues.Where(v => v != null))
                {
                    PendingVenues.Add(venue);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to load venues", "OK");
            }
        }
        
        //Command to Load the Selected Venue
        private async void LoadSelectedVenue(Guid venueId)
        {
            try
            {
                await Shell.Current.DisplayAlert("Prompt", $"number of Images{VenueImageUrls.Count()}", "OK");
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

						var newUrls = new ObservableCollection<string>(venue.ImageUrls);
						VenueImageUrls = newUrls;

						await Shell.Current.DisplayAlert("Prompt", $"number of Images{VenueImageUrls.Count()} AFTERRRRR", "OK");

					}
				}

                if (venue != null)
                {
                    SelectedVenue = venue;


                    //if (!string.IsNullOrEmpty(venue.ImageUrl))
                    //{
                    //    VenueImageUrls = venue.ImageUrl
                    //        .Split(',')
                    //        .Where(url => !string.IsNullOrWhiteSpace(url))
                    //        .ToList();

                    // Force property change notification

                    //    // Debug output
                    //    Debug.WriteLine($"Found {VenueImageUrls.Count} image URLs");
                    //    foreach (var url in VenueImageUrls)
                    //    {
                    //        Debug.WriteLine($"Image URL: {url}");
                    //    }
                    //}
                    //else
                    //{
                    //    VenueImageUrls = new List<string>();
                    //    Debug.WriteLine("No image URLs found");
                    //}
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message , "OK");
            }
        }

        //Command to Accept the Venue
        [RelayCommand]
        private async Task AcceptAsync(Venue venue)
        {
            // Use either the passed venue or the selected venue
           // venue = venue ?? SelectedVenue;
            
            try
            {
                if (venue == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Venue is null. Command parameter not properly passed.", "OK");
                    return;
                }

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
                await Shell.Current.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        //Command to Delete the venue
        [RelayCommand]
        private async Task DeleteAsync(Venue venue)
        {
            // Use either the passed venue or the selected venue
          //  venue = venue ?? SelectedVenue;
            
            if (venue == null)
            {
                await Shell.Current.DisplayAlert("Error", "Selected venue not found", "OK");
                return;
            }

            try
            {
                bool confirmed = await Shell.Current.DisplayAlert("Confirm", 
                    "Are you sure you want to reject this venue?", "Yes", "No");
                
                if (!confirmed)
                    return;
                
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