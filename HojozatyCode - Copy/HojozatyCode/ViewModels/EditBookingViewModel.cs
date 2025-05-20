using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using Supabase;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace HojozatyCode.ViewModels
{
	[QueryProperty(nameof(Booking), "Booking")]
	[QueryProperty(nameof(Venue), "Venue")]
	public partial class EditBookingViewModel : ObservableObject
	{
		//To store the Booking Object
		[ObservableProperty]
		private BookingWithVenue booking;	
		
		//To store the Booking Object
		[ObservableProperty]
		private ObservableCollection<Booking> venueBookings;

	

		//To store the Venue Object
		[ObservableProperty]
		private Venue venue;

		//To store the all selected services for this booking
		[ObservableProperty]
		private ObservableCollection<BookingService> selectedServices = new();

		private ObservableCollection<Service> services = new();

		//To store the Start Date time for the booking
		[ObservableProperty]
		private DateTime startDate;

		//To store the Start Time for the Booking
		[ObservableProperty]
		private TimeSpan startTime;

		//To Sotre the End date time for the booking
		[ObservableProperty]
		private DateTime endDate;

		//To store the end time for the booking
		[ObservableProperty]
		private TimeSpan endTime;

		//Conditioner to check if the Venue has Fixed duration or not
		public bool CanEditEndDate => Venue!= null && Venue.IsFixedDuration == true;

	
		partial void OnBookingChanged(BookingWithVenue value)
		{
			try
			{
				StartDate = value.StartDateTime.Date;
				StartTime = value.StartDateTime.TimeOfDay;
				EndDate = value.EndDateTime.Date;
				EndTime = value.EndDateTime.TimeOfDay;

				LoadBookingServices();
				LoadBookingsAsync(venue.VenueId);
			}
			catch (Exception ex) 
			{
				Shell.Current.DisplayAlert("Error", ex.Message, "OK");
			}
		}

		private async void LoadBookingServices()
		{
			try
			{
				var response = await SupabaseConfig.SupabaseClient
					.From<BookingService>()
					.Where(x => x.BookingId == Booking.BookingId)
					.Get();

				SelectedServices.Clear();


				foreach (var bookingService in response.Models)
				{
					// Fetch related service
					var serviceResponse = await SupabaseConfig.SupabaseClient
						.From<Service>()
						.Where(s => s.ServiceId == bookingService.ServiceId)
						.Single();

					var service = serviceResponse;
					bookingService.Name = service?.ServiceName ?? "Unknown Service";
					SelectedServices.Add(bookingService);
				}


			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
			}
		}

		private DateTime CalculateEndTime()
		{
			DateTime newEnd = DateTime.Now;

			//Check if the venue has fixed time or not (To store the right ended time)
			if (Venue.IsFixedDuration != null && Venue.IsFixedDuration == true)
			{
				var duration = (int)Venue.FixedDurationInHours;
				newEnd = EndDate = StartDate.AddHours(duration);
			}

			else
			{

				//Check if the ended time isn't before the started time
				if (EndTime > StartTime)
				{
					newEnd = EndDate = StartDate + EndTime;
				}
				else
				{
					Shell.Current.DisplayAlert("Warning", "Please Select Appropriate Ending Time", "OK");
					return DateTime.Now;
				}
			}

			return newEnd;
		}

		[RelayCommand]
		public async Task LoadBookingsAsync(Guid venueId)
		{

			// Fetch all bookings where venue_id matches
			var response = await SupabaseConfig.SupabaseClient
				.From<Booking>()
				.Where(b => b.VenueId == venueId)
				.Get();

			if (response.Models != null)
				VenueBookings = new ObservableCollection<Booking>(response.Models);

		}

		private bool IsDateTimeAvailable(DateTime newBookingStart, DateTime newBookingEnd)
		{
			if (VenueBookings == null)
				return true;

			foreach (var booking in VenueBookings)
			{
				//if it's not available return false
				if (booking.StartDateTime < newBookingEnd && newBookingStart < booking.EndDateTime)
				{
					return false;
				}
			}

			return true;
		}

		//Command to check the Availability Booking for this DateTime
		[RelayCommand]
		private async Task CheckAvailability()
		{
			var newBookingStart = StartDate = StartDate + StartTime;

			var newBookingEnd = CalculateEndTime();

			if (newBookingEnd == DateTime.Now)
			{
				return;
			}

			if (!IsDateTimeAvailable(StartDate.AddHours(3), newBookingEnd.AddHours(3)))
			{
				await Shell.Current.DisplayAlert("Warning", $"Venue is reserved  at this time", "OK");
				return;
			}
			else
			{
				await Shell.Current.DisplayAlert("Warning", $"Venue is available at this time", "OK");
				return;
			}
		}

		[RelayCommand]
		private async Task UpdateBooking()
		{
			try
			{
				var newStart = StartDate.Add(StartTime);
				var newEnd = CalculateEndTime();

				if (newEnd == DateTime.Now)
				{
					// Invalid end time due to logic in CalculateEndTime
					return;
				}

				// Check if selected time is available (excluding current booking)
				bool isAvailable = true;

				foreach (var booking in VenueBookings)
				{
					// Skip the current booking being edited
					if (booking.BookingId == Booking.BookingId)
						continue;

					if (booking.StartDateTime < newEnd && newStart < booking.EndDateTime)
					{
						isAvailable = false;
						break;
					}
				}

				if (!isAvailable)
				{
					await Shell.Current.DisplayAlert("Warning", "This time slot is already booked.", "OK");
					return;
				}

				// Proceed with updating the booking
				Booking.StartDateTime = newStart;
				Booking.EndDateTime = newEnd;

				var updatedBooking = new Booking
				{
					BookingId = Booking.BookingId,
					CreatedAt = Booking.CreatedAt,
					EndDateTime = newEnd.AddHours(3),
					StartDateTime = newStart.AddHours(3),
					TotalPrice = Booking.TotalPrice,
					VenueId = Booking.VenueId,
				};

				var response = await SupabaseConfig.SupabaseClient
					.From<Booking>()
					.Where(x => x.BookingId == Booking.BookingId)
					.Update(updatedBooking);

				if (response.ResponseMessage.IsSuccessStatusCode)
				{
					await Shell.Current.DisplayAlert("Success", "Booking updated successfully.", "OK");
					await Shell.Current.GoToAsync("..");
				}
				else
				{
					await Shell.Current.DisplayAlert("Error", "Failed to update booking.", "OK");
				}
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
			}
		}

	}
}
