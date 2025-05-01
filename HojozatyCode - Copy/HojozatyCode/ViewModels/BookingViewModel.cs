using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using Microsoft.Maui.Devices;
using Supabase.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;


namespace HojozatyCode.ViewModels
{

	public partial class BookingViewModel : ObservableObject, IQueryAttributable
	{

		[ObservableProperty]
		private string venueIdRaw;

		[ObservableProperty]
		private Guid venueId;

		// Selected venue for booking
		[ObservableProperty]
		private Venue selectedVenue;

		[ObservableProperty]
		private ObservableCollection<HostRules> hostRulesVenue = new ObservableCollection<HostRules>();

		public bool HasHostRules => HostRulesVenue != null && HostRulesVenue.Count > 0;
		public bool NoHostRules => !HasHostRules;


		[ObservableProperty]
		private ObservableCollection<ServiceItem> servicesVenue = new ObservableCollection<ServiceItem>();

		[ObservableProperty]
		private ObservableCollection<string> imageUrls = new ObservableCollection<string>();
		public bool HasServices => ServicesVenue != null && ServicesVenue.Count > 0;
		public bool NoServices => !HasServices;

		[ObservableProperty]
		private string errorMessage;

		//Properties To Deal with the Calendar

		// List to store all bookings for the selected venue
		[ObservableProperty]
		private ObservableCollection<Booking> _venueBookings = new();

		[ObservableProperty]
		private DateTime selectedDate = DateTime.Today;

		[ObservableProperty]
		private TimeSpan selectedTime = DateTime.Now.TimeOfDay;

		public DateTime SelectedDateTime => SelectedDate.Date + SelectedTime;

		partial void OnSelectedDateChanged(DateTime value)
		{
			OnPropertyChanged(nameof(SelectedDateTime));
		}

		partial void OnSelectedTimeChanged(TimeSpan value)
		{
			OnPropertyChanged(nameof(SelectedDateTime));
		}

		[RelayCommand]
		public async Task LoadBookingsAsync(Guid venueId)
		{
			VenueId = venueId;

			// Fetch all bookings where venue_id matches
			var response = await SupabaseConfig.SupabaseClient
				.From<Booking>()
				.Where(b => b.VenueId == venueId)
				.Get();

			if (response.Models != null)
				VenueBookings = new ObservableCollection<Booking>(response.Models);

			await Shell.Current.DisplayAlert("Prompt", "Inside The Load Bookings Async", "OK");

			foreach (var book in VenueBookings) 
			{
				await Shell.Current.DisplayAlert("Prompt", $"{book.StartDateTime} To {book.EndDateTime}", "OK");
			}

		}

		/// <summary>
		/// Check if the selected date and time is available
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		private async Task<bool> IsDateTimeAvailable(DateTime newBookingStart, DateTime newBookingEnd)
		{
			if (VenueBookings == null)
				return true;

			foreach (var booking in VenueBookings)
			{
				if (booking.StartDateTime < newBookingEnd && newBookingStart < booking.EndDateTime)
				{
					return false;
				}
			}

			return true;
		}
		/// <summary>
		/// Create a new booking
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>


		[RelayCommand]
		public async Task CreateBooking()
		{
			var newBookingStart = SelectedDateTime;
			var newBookingEnd = SelectedDateTime.AddHours(1);

			await Shell.Current.DisplayAlert("Prmpt", $"Selected date time value was {SelectedDateTime}", "OK");

			if (!await IsDateTimeAvailable(newBookingStart, newBookingEnd))
			{
				await Shell.Current.DisplayAlert("Prompt", $"There was a booking from {newBookingStart} to {newBookingEnd}", "OK");
				return ; // Can't book if not available
			}

			await Shell.Current.DisplayAlert("Prompt", $"Start to Booking Date", "OK");

			var tempAuth = SupabaseConfig.SupabaseClient.Auth.CurrentUser;

			// Create a new Booking object
			var newBooking = new Booking
			{				
				BookingId = Guid.NewGuid(),
				UserId = Guid.Parse(tempAuth.Id),
				VenueId = VenueId,
				StartDateTime = SelectedDateTime,
				EndDateTime = SelectedDateTime.AddHours(1), // Booking is 1 hour
				Status = "confirmed",
				TotalPrice = 100, // Example price (you can calculate based on venue)
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow
			};

			var response = await SupabaseConfig.SupabaseClient.From<Booking>().Insert(newBooking);

			 await Shell.Current.DisplayAlert("Prompt", $"The Date is Booked", "OK");

			if (response.ResponseMessage.IsSuccessStatusCode)
			{
				VenueBookings.Add(newBooking); // Update local list
				return ;
			}

			return ;
		}
	

		// Constructor - private to enforce singleton pattern
		public BookingViewModel()
		{
			// Initialize if needed
		}
		public async void ApplyQueryAttributes(IDictionary<string, object> query)
		{
			// نحصل على البيانات المرسلة باسم "SelectedVenue"
			if (query.TryGetValue("SelectedVenue", out var venueData))
			{
				SelectedVenue = venueData as Venue;

				if (SelectedVenue != null)
				{
					ImageUrls = SelectedVenue.ImageUrl.Split(',').ToObservableCollection();
					await LoadHostRules();
					await LoadServices();
					await LoadBookingsAsync(SelectedVenue.VenueId);
				}
			}
		}

		private async Task LoadHostRules()
		{
			try
			{
				if (SelectedVenue == null || SelectedVenue.VenueId == Guid.Empty)
				{
					ErrorMessage = "Invalid Venue ID";
					return;
				}

				HostRulesVenue.Clear();

				//Get Host Rules Id's related to this Venue 
				var hostRulesVenues = await SupabaseConfig.SupabaseClient
						   .From<HostRulesVenues>() // هذا يمثل الجدول الوسيط
						   .Where(x => x.VenueId == SelectedVenue.VenueId)
						   .Get();

				var hostRuleIds = hostRulesVenues.Models.Select(x => x.HostRuleId).ToList();

				if (hostRuleIds.Count == 0)
				{
					await Shell.Current.DisplayAlert("Prompt", $"The Count of Host rules was {hostRuleIds.Count}", "OK");
					HostRulesVenue = new ObservableCollection<HostRules>();
					return;
				}

				// Step 2: Get the HostRules based on these IDs
				foreach (var hostRuleId in hostRuleIds)
				{
					var hostRulesForId = await SupabaseConfig.SupabaseClient
									.From<HostRules>()
									.Where(x => x.HostRuleId == hostRuleId)
									.Get();

					HostRulesVenue.Add(hostRulesForId.Model);
				}

				OnPropertyChanged(nameof(HasHostRules));
				OnPropertyChanged(nameof(NoHostRules));

			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", $"There was an {ex} Exception", "OK");
			}
		}

		private async Task LoadServices()
		{
			try
			{
				if (SelectedVenue == null || SelectedVenue.VenueId == Guid.Empty)
				{
					ErrorMessage = "Invalid Venue ID";
					return;
				}

				ServicesVenue.Clear();

				//Get Host Rules Id's related to this Venue 
				var servicesVenues = await SupabaseConfig.SupabaseClient
						   .From<VenueServices>() // هذا يمثل الجدول الوسيط
						   .Where(x => x.VenueId == SelectedVenue.VenueId)
						   .Get();

				var serviceVenueId = servicesVenues.Models.ToList();

				if (serviceVenueId.Count == 0)
				{
					await Shell.Current.DisplayAlert("Prompt", $"The Count of Services was {serviceVenueId.Count}", "OK");
					ServicesVenue = new ObservableCollection<ServiceItem>();
					return;
				}


				var serviceDisplayList = new ObservableCollection<ServiceItem>();

				// Step 2: Get the HostRules based on these IDs
				foreach (var serviceId in serviceVenueId)
				{
					var serviceForId = await SupabaseConfig.SupabaseClient
									.From<Service>()
									.Where(x => x.ServiceId == serviceId.ServiceId)
									.Get();

					if (serviceForId.Model != null)
					{
						serviceDisplayList.Add(new ServiceItem
						{
							Name = serviceForId.Model.ServiceName,
							Description = serviceForId.Model.Description,
							Price = serviceId.PricePerUnit // نأخذ السعر من الجدول الوسيط
						});
					}
				}
				ServicesVenue = serviceDisplayList;


				OnPropertyChanged(nameof(HasServices));
				OnPropertyChanged(nameof(NoServices));

			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", $"There was an {ex} Exception", "OK");
			}
		}

		[RelayCommand]
		private async Task GoToBookingCalendarPage() 
		{
			await Shell.Current.GoToAsync(nameof(Pages.BookingCalendarPage));	
		}
	}
}