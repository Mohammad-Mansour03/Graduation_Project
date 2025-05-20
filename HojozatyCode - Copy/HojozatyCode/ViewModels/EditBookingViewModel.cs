using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using Supabase;
using System;
using System.Collections.ObjectModel;
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

		//To store the Venue Object
		[ObservableProperty]
		private Venue venue;

		//To store the all selected services for this booking
		[ObservableProperty]
		private ObservableCollection<Service> selectedServices = new();

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

				foreach (var bookingService in response.Models)
				{
					var serviceResponse = await SupabaseConfig.SupabaseClient
						.From<Service>()
						.Filter("service_id", Supabase.Postgrest.Constants.Operator.Equals, bookingService.ServiceId)
						.Single();

					if (serviceResponse != null)
						SelectedServices.Add(serviceResponse);
				}
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
			}
		}


		[RelayCommand]
		private async Task UpdateBooking()
		{
			await Shell.Current.DisplayAlert("Prompt", "Inside the Update Booking", "OK");
			try
			{
				Booking.StartDateTime = StartDate.Add(StartTime);

				if (Venue.IsFixedDuration != true)
					Booking.EndDateTime = EndDate.Add(EndTime);

				var updateBooking = new Booking
				{
					BookingId = Booking.BookingId,
					CreatedAt = Booking.CreatedAt,
					EndDateTime = Booking.EndDateTime.AddHours(3),
					StartDateTime = Booking.StartDateTime.AddHours(3),
					TotalPrice = Booking.TotalPrice,
					VenueId = Booking.VenueId,
				};

				var response = await SupabaseConfig.SupabaseClient
					.From<Booking>()
					.Where(x => x.BookingId == Booking.BookingId)
					.Update(updateBooking);

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
