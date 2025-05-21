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
		
		//To store the All Bookings for this Venue
		[ObservableProperty]
		private ObservableCollection<Booking> venueBookings = new();

		//To store the Venue Object
		[ObservableProperty]
		private Venue venue;

		//To store the all selected services for this booking
		[ObservableProperty]
		private ObservableCollection<BookingService> selectedServices = new();

		//Properety to store the StartDate
		[ObservableProperty]
		private DateTime startDate;

		//Properety to store the Start Time
		[ObservableProperty]
		private TimeSpan startTime;

		//Properety to store the EndDate
		[ObservableProperty]
		private DateTime endDate;

		//Properety to store the EndTime
		[ObservableProperty]
		private TimeSpan endTime;

		//Conditioners to check if the venue has fixed duration or not 
		public bool CanEditEndDate => Venue != null && Venue.IsFixedDuration != true;
		public bool IsFixedDuration => Venue != null && Venue.IsFixedDuration == true;

		
		partial void OnBookingChanged(BookingWithVenue value)
		{
			try
			{
				StartDate = value.StartDateTime.Date;
				StartTime = value.StartDateTime.TimeOfDay;
				EndDate = value.EndDateTime.Date;
				EndTime = value.EndDateTime.TimeOfDay;

				LoadBookingServices();
				LoadBookingsAsync(value.VenueId);
			}
			catch (Exception ex) 
			{
				Shell.Current.DisplayAlert("Error inside OnBookingChanged", ex.Message, "OK");
			}
		}

		//Method to load the All Services related to the Booking
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
				await Shell.Current.DisplayAlert("Error Inside Load Booking Services", ex.Message, "OK");
			}
		}

		//Method Load the All Booking Related to this Venue
		private async Task LoadBookingsAsync(Guid venueId)
		{
			try
			{
				// Fetch all bookings where venue_id matches
				var response = await SupabaseConfig.SupabaseClient
					.From<Booking>()
					.Where(b => b.VenueId == venueId)
					.Get();

				if (response.Models != null)
					VenueBookings = new ObservableCollection<Booking>(response.Models);
			}
			catch (Exception ex) 
			{
				await Shell.Current.DisplayAlert("Error Inside LoadBooking Async" , ex.Message, "OK");
			}
		}

		//Method to calculate the End time
		private DateTime CalculateEndTime()
		{
			try
			{
				if (Venue == null)
					return DateTime.Now;

				var start = StartDate.Date + StartTime;

				if (Venue.IsFixedDuration == true)
				{
					return start.AddHours(Venue.FixedDurationInHours ?? 0);
				}

				if (EndTime <= StartTime)
				{
					Shell.Current.DisplayAlert("Warning", "End time must be after start time.", "OK");
					return DateTime.Now;
				}

				return start.Date + EndTime;
			}
			catch (Exception ex)
			{
				 Shell.Current.DisplayAlert("Error Inside Calculate End Time Async", ex.Message, "OK");
				return DateTime.Now;
			}
		}

		//Method to Check if the interval is Available or not (Return false when there a conflict)
		private bool IsDateTimeAvailable(DateTime newStart, DateTime newEnd)
		{
			try
			{
				foreach (var booking in VenueBookings)
				{
					if (booking.BookingId == Booking.BookingId)
						continue;

					if (booking.StartDateTime < newEnd && newStart < booking.EndDateTime)
					{
						return false;
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				 Shell.Current.DisplayAlert("Error Inside IsDateTimeAvailablr", ex.Message, "OK");
				return false;
			}
		}


		//Command to check the Availability Booking for this DateTime
		[RelayCommand]
		private async Task CheckAvailability()
		{
			try
			{
				var newStart = StartDate.Date + StartTime;
				var newEnd = CalculateEndTime();

				if (newEnd <= newStart)
				{
					await Shell.Current.DisplayAlert("Warning", "End time must be after start time.", "OK");
					return;
				}

				if (!IsDateTimeAvailable(newStart, newEnd))
				{
					await Shell.Current.DisplayAlert("Not Available", "This slot is already booked.", "OK");
					return;
				}

				await Shell.Current.DisplayAlert("Available", "This slot is available.", "OK");
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error Inside Check Availability Async", ex.Message, "OK");
			}
		}

		//Command to update the Booking
		[RelayCommand]
		private async Task UpdateBooking()
		{
			try
			{
				var newStart = StartDate.Date + StartTime;
				var newEnd = CalculateEndTime();

				if (newEnd <= newStart)
				{
					await Shell.Current.DisplayAlert("Warning", "End time must be after start time.", "OK");
					return;
				}

				if (!IsDateTimeAvailable(newStart, newEnd))
				{
					await Shell.Current.DisplayAlert("Warning", "This time slot is already booked.", "OK");
					return;
				}

				Booking.StartDateTime = newStart;
				Booking.EndDateTime = newEnd;

				var userId = SupabaseConfig.SupabaseClient.Auth.CurrentUser.Id;

				Guid userGuidId = Guid.Parse(userId);

				var updatedBooking = new Booking
				{
					BookingId = Booking.BookingId,
					UserId = userGuidId,
					CreatedAt = Booking.CreatedAt,
					StartDateTime = newStart.AddHours(3), // Time zone offset
					EndDateTime = newEnd.AddHours(3),
					VenueId = Booking.VenueId,
					TotalPrice = Booking.TotalPrice
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
				await Shell.Current.DisplayAlert("Error Inside Update ", ex.Message, "OK");
			}
		}
	}

}

