using HojozatyCode.Models;
using HojozatyCode.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Maui.Controls;
using System;
using HojozatyCode.Pages;
using Microsoft.Maui.Devices;
using System.Reactive.Linq;
using CommunityToolkit.Maui.Core.Extensions;

namespace HojozatyCode.ViewModels
{
    public partial class MySpaceViewModel : ObservableObject
    {
		// Property to check if there are no spaces
		[ObservableProperty]
		private bool isEmpty;

		//Collection to store the Owner's venues
		[ObservableProperty]
        private ObservableCollection<Venue> venues;

        //Boolean variable to check if the Venues was loading or no
        [ObservableProperty]
        private bool isLoading = false;

        
        //The Constructor
        public MySpaceViewModel()
        {
            Venues = new ObservableCollection<Venue>();
        }

		// Method to Load the Owner's Venues
		public async Task LoadVenuesAsync()
		{
			if (IsLoading)
				return;

			if (SupabaseConfig.SupabaseClient == null)
				await SupabaseConfig.InitializeAsync();

			IsLoading = true;

			try
			{
				var session = SupabaseConfig.SupabaseClient.Auth.CurrentSession;

				if (session == null || session.User == null)
				{
					IsLoading = false;
					await Shell.Current.DisplayAlert("Error", "No User logged in", "OK");
					return;
				}

				var userId = session.User.Id;
				Guid userIdGuid = Guid.Parse(userId);

				var response = await SupabaseConfig.SupabaseClient
					.From<Venue>()
					.Select("*")
					.Where(v => v.OwnerId == userIdGuid)
					.Get();

				Venues.Clear();

				foreach (var venue in response.Models)
				{
					try
					{
						venue.DisplayAddress = await TryGetDisplayAddressAsync(venue.Location, venue.City);
					}
					catch (Exception ex)
					{
						await Shell.Current.DisplayAlert("Error", $"Error during reverse geocoding: {ex.Message}", "OK");
						venue.DisplayAddress = !string.IsNullOrEmpty(venue.City) ? venue.City : venue.Location;
					}

					Venues.Add(venue);
				}

				IsEmpty = Venues.Count == 0;
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", $"Error loading venues: {ex.Message}", "OK");
			}
			finally
			{
				IsLoading = false;
			}
		}

		private async Task<string> TryGetDisplayAddressAsync(string location, string cityFallback)
		{
			try
			{
				var parts = location.Split(',');
				if (parts.Length != 2 ||
					!double.TryParse(parts[0], out double latitude) ||
					!double.TryParse(parts[1], out double longitude))
					return location;

				var geocoder = Microsoft.Maui.Devices.Sensors.Geocoding.Default;
				var placemarksResult = await geocoder.GetPlacemarksAsync(latitude, longitude);
				var placemarks = placemarksResult?.ToList(); // Prevent disposal issue
				var placemark = placemarks?.FirstOrDefault();

				if (placemark == null)
					return !string.IsNullOrEmpty(cityFallback) ? cityFallback : location;

				var addressParts = new List<string>
		{
			placemark.SubThoroughfare,
			placemark.Thoroughfare,
			placemark.SubLocality,
			placemark.Locality,
			placemark.SubAdminArea != placemark.Locality ? placemark.SubAdminArea : null,
			placemark.AdminArea
		};

				var address = string.Join(", ", addressParts.Where(p => !string.IsNullOrEmpty(p)));

				if (string.IsNullOrEmpty(address))
					address = !string.IsNullOrEmpty(placemark.CountryName) ? placemark.CountryName : location;

				if (!string.IsNullOrEmpty(placemark.PostalCode))
					address += $" {placemark.PostalCode}";

				return address;
			}
			catch
			{
				return !string.IsNullOrEmpty(cityFallback) ? cityFallback : location;
			}
		}


		//Command to delete the venue
		[RelayCommand]
        public async Task DeleteVenue(Venue venue)
        {
            if (venue == null)
                return;

            //Prompt for user to resure the delete
            bool isConfirmed = await Shell.Current.DisplayAlert(
                "Delete Confirmation",
               $"Are your sure you want to delete : {venue.VenueName}",
                "Yes", "No");

            if (!isConfirmed)
                return;

            try
            {
                await SupabaseConfig.SupabaseClient
                   .From<Venue>()
                   .Where(v => v.VenueId == venue.VenueId)
                   .Delete();

                Venues.Remove(venue);


                await Shell.Current.DisplayAlert("Done", "The Space Deleted Succesfully", "OK");

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"There is an error occur while deleting the space {ex.Message}", "OK");
            }
        }

		public IRelayCommand<Venue> EditVenueCommand => new RelayCommand<Venue>(GoToEditPage);

		private async void GoToEditPage(Venue selectedVenue)
		{
			try
			{
				var navParams = new Dictionary<string, object>
							{
								{ "VenueToEdit", selectedVenue }
							};

				await Shell.Current.GoToAsync(nameof(EditPage), navParams);
			}
			catch (Exception ex) 
			{
				await Shell.Current.DisplayAlert("Error" , ex.Message, "OK");	
			}
		}

		private async Task<ObservableCollection<Booking>> LoadVenueBooking(Guid venueId) 
		{
			var bookings = await SupabaseConfig.SupabaseClient
				.From<Booking>()
				.Where(x => x.VenueId == venueId)
				.Get();

			if (bookings.Models.Count > 0)
				return bookings.Models.ToObservableCollection();

			return null;
		}


		[RelayCommand]
		private async Task ViewBookings(Venue venue)
		{
			var bookings = await LoadVenueBooking(venue.VenueId);
			if (bookings != null && bookings.Count > 0)
			{
				await Shell.Current.Navigation.PushModalAsync(new ViewBookingsPopup(bookings));
			}
			else
			{
				await Shell.Current.DisplayAlert("تنبيه", "لا توجد حجوزات لهذا المكان.", "موافق");
			}
		}

		//Command to navigate me to the Add Space Page
		[RelayCommand]
		private async Task GoToAddSpace()
		{
			await Shell.Current.GoToAsync(nameof(Pages.SpaceTypeSelectionPage));
		}

		//Command to navigate me to the my space page (When user click on the Cancel)
		private async Task GoToMySpaceForCancel()
		{
			await Shell.Current.GoToAsync(nameof(Pages.MySpace));
		}

	}

}
