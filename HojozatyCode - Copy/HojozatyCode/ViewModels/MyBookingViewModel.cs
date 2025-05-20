using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HojozatyCode.ViewModels
{
    public partial class MyBookingViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<BookingWithVenue> myBookings = new();

        private Guid userId;



        public MyBookingViewModel() 
        {
            LoadBooking();
        }


		[RelayCommand]
		private async Task DeleteBooking(BookingWithVenue booking) 
		{
			try
			{
				string warningMessage = booking.CancellationPolicy switch
				{
					"Flexible" => "According to the flexible cancellation policy, you will be refunded the full amount you paid.",
					"Moderate" => "According to the moderate cancellation policy, you will only be refunded 50% of the amount you paid.",
					"Strict" => "According to the moderate cancellation policy, you will only be refunded 30% of the amount you paid."
				};

				var confirm = await Shell.Current.DisplayAlert(
						"Delete Booking",
						$"{warningMessage}\n\nAre you sure you want to delete this booking?",
						"Yes", "Cancel");
				
				if (!confirm)
					return;

				await SupabaseConfig.SupabaseClient
					.From<Booking>()
					.Where(b => b.BookingId == booking.BookingId)
					.Delete();

				MyBookings.Remove(booking);
				await Shell.Current.DisplayAlert("Deleted", "Booking deleted successfully", "OK");

			}
			catch (Exception ex) 
			{
				await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
			}

		}

		[RelayCommand]
		private async Task EditBookingAsync(BookingWithVenue booking)
		{
			try
			{
				var venue = await SupabaseConfig.SupabaseClient
					.From<Venue>()
					.Where(v => v.VenueId == booking.VenueId)
					.Single();

			await Shell.Current.GoToAsync(nameof(Pages.EditBooking), true, new Dictionary<string, object>
			{
				{ "Booking", booking },
				{ "Venue", venue }
			});
			}
			catch (Exception ex) 
			{
				await Shell.Current.DisplayAlert("Error inside the EditBookingAsync" , ex.Message, "OK");	
			}
		}


		//Method to load the all booking related to the user
		private async Task LoadBooking() 
        {
            try
            {
                var userIdString = SupabaseConfig.SupabaseClient.Auth.CurrentUser.Id;

                if (userIdString == null) 
                {
                    await Shell.Current.DisplayAlert("Error", "Please Login to can see your bookings", "OK");
                    return;
                }

                userId = Guid.Parse(userIdString);

                var bookingResults = await SupabaseConfig.SupabaseClient
                    .From<Booking>()
                    .Where(x => x.UserId == userId)
                    .Get();

                MyBookings.Clear();

                if (bookingResults == null) 
                {
					await Shell.Current.DisplayAlert("Error", "There is no any booking for you", "OK");
                    return;
				}

                foreach (var booking in bookingResults.Models) 
                {
                    var venue = await SupabaseConfig.SupabaseClient
                        .From<Venue>()
                        .Where(x => x.VenueId == booking.VenueId)
                        .Single();

					venue.DisplayAddress = await GetActualAddres(venue);

                    MyBookings.Add(new BookingWithVenue
                    {
                        VenueId = venue.VenueId,
                        BookingId = booking.BookingId,
                        Capacity = venue.Capacity,
                        CreatedAt = booking.CreatedAt,
                        EndDateTime = booking.EndDateTime,
                        StartDateTime = booking.StartDateTime,
                        TotalPrice = booking.TotalPrice,
                        VenueEmail = venue.VenueEmail,
                        Location = venue.DisplayAddress,
                        VenueName = venue.VenueName,
                        ImageUrls = venue.ImageUrls,
						CancellationPolicy = venue.CancellationPolicy,
                    });
                }

			}
			catch (Exception ex) 
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

		//Method to return the actual address for the venue
		public async Task<string> GetActualAddres(Venue venue)
		{
			string displayLocation = string.Empty;

			if (venue == null || string.IsNullOrEmpty(venue.Location))
			{
				displayLocation = "Location unavailable";
				return displayLocation;
			}

			try
			{
				// Parse the location string assuming format is "latitude,longitude"
				var parts = venue.Location.Split(',');
				if (parts.Length != 2 || !double.TryParse(parts[0], out double latitude) ||
					!double.TryParse(parts[1], out double longitude))
				{
					// If not in expected format, just display the raw value
					displayLocation = venue.Location;
					return displayLocation;
				}

				// Use MAUI Geocoding
				var geocoder = Microsoft.Maui.Devices.Sensors.Geocoding.Default;
				var placemarks = await geocoder.GetPlacemarksAsync(latitude, longitude);
				var placemark = placemarks?.FirstOrDefault();

				if (placemark != null)
				{
					// Format the address in a more detailed, readable way
					var addressParts = new List<string>();

					// Street-level details
					if (!string.IsNullOrEmpty(placemark.SubThoroughfare))
						addressParts.Add(placemark.SubThoroughfare);

					if (!string.IsNullOrEmpty(placemark.Thoroughfare))
						addressParts.Add(placemark.Thoroughfare);

					// Neighborhood/district
					if (!string.IsNullOrEmpty(placemark.SubLocality))
						addressParts.Add(placemark.SubLocality);

					// City
					if (!string.IsNullOrEmpty(placemark.Locality))
						addressParts.Add(placemark.Locality);

					// County/district
					if (!string.IsNullOrEmpty(placemark.SubAdminArea) && placemark.SubAdminArea != placemark.Locality)
						addressParts.Add(placemark.SubAdminArea);

					// State/province
					if (!string.IsNullOrEmpty(placemark.AdminArea))
						addressParts.Add(placemark.AdminArea);

					// Build the display string
					displayLocation = string.Join(", ", addressParts.Where(p => !string.IsNullOrEmpty(p)));

					// Fallbacks if we couldn't build a good address
					if (string.IsNullOrEmpty(displayLocation))
						displayLocation = !string.IsNullOrEmpty(placemark.CountryName) ?
							placemark.CountryName :
							venue.Location;

					// Add postal code as additional information if available
					if (!string.IsNullOrEmpty(placemark.PostalCode))
						displayLocation += $" {placemark.PostalCode}";
				}
				else
				{
					// Try to use the City field from the Venue if available
					displayLocation = !string.IsNullOrEmpty(venue.City) ?
						venue.City :
						venue.Location;
				}
				return displayLocation;
			}
			catch (Exception ex)
			{
				// Log the error and fall back to coordinates or city name
				await Shell.Current.DisplayAlert("Error", $"Error during reverse geocoding: {ex.Message}", "OK");
				displayLocation = !string.IsNullOrEmpty(venue.City) ?
					venue.City :
					venue.Location;
				return null;
			}



		}

	}
}
