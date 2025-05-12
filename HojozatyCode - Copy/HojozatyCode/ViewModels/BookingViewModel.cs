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
using System.Diagnostics;
using System.Threading.Tasks;


namespace HojozatyCode.ViewModels
{

	public partial class BookingViewModel : ObservableObject, IQueryAttributable
	{

		//To deal with venue id that come from another page
		[ObservableProperty]
		private string venueIdRaw;
		
		//To store the VenueId that come from another page and dealing with it in quireies
		[ObservableProperty]
		private Guid venueId;

		// Selected venue for booking
		[ObservableProperty]
		private Venue selectedVenue;

		//ObservableCollection to store the host rules that related to the venue
		[ObservableProperty]
		private ObservableCollection<HostRules> hostRulesVenue = new ObservableCollection<HostRules>();

		//Conditioners to check if the Venue has host rules or not
		public bool HasHostRules => HostRulesVenue != null && HostRulesVenue.Count > 0;
		public bool NoHostRules => !HasHostRules;

		//ObservableCollection to store the all services that related to the venue
		[ObservableProperty]
		private ObservableCollection<ServiceItem> servicesVenue = new ObservableCollection<ServiceItem>();

		//Conditionires to check if the venue has services or not
		public bool HasServices => ServicesVenue != null && ServicesVenue.Count > 0;
		public bool NoServices => !HasServices;
		
		//To display there is an error when user choose something wrong
		[ObservableProperty]
		private string errorMessage;
		
		//Conditioner to check if the venue has fixing time or no
		[ObservableProperty]
		private bool hasFixedTime;

		//Properties To Deal with the Calendar

		// List to store all bookings for the selected venue
		[ObservableProperty]
		private ObservableCollection<Booking> _venueBookings = new();

		//Date time to store the Started Date that user choosen
		[ObservableProperty]
		private DateTime selectedDate = DateTime.Today;

		//Time Span to store the Started time that user choosen
		[ObservableProperty]
		private TimeSpan selectedTime = DateTime.Now.TimeOfDay;
		
		//Time Span to store the Ended time that user choosen
		[ObservableProperty]
		private TimeSpan endedTime;

		[ObservableProperty]
		private DateTime endedDateTime;

		//Date time to store the Started Date and time that user choosen
		[ObservableProperty]
		public DateTime selectedDateTime;

		//To Store the Current Booking ID
		[ObservableProperty]
		private Guid selectedBookingId;
		
		//To store the Current User ID
		[ObservableProperty]
		private Guid currentUserId;

		//To Store the Services the user added for his booking
		[ObservableProperty]
		ObservableCollection<BookingService> bookingServices = new ObservableCollection<BookingService>();

		[ObservableProperty]
		private double totalPrice;
		

		//To delete the service from booking
		[RelayCommand]
		private async Task DeleteService(ServiceItem bookingService)
		{
			try
			{
				// 1. Remove from database
				await SupabaseConfig.SupabaseClient
					.From<BookingService>()
					.Where(bs => bs.BookingId == SelectedBookingId && bs.ServiceId == bookingService.ServiceId)
					.Delete();

				// 2. Update booking total price
				var booking = await SupabaseConfig.SupabaseClient
					.From<Booking>()
					.Where(b => b.BookingId == SelectedBookingId)
					.Get();

				TotalPrice = booking.Model.TotalPrice -= (bookingService.Price * bookingService.Quantity);
				await booking.Model.Update<Booking>();

				var tempBookingService = new BookingService
				{
					ServiceId = bookingService.ServiceId,
					BookingId = SelectedBookingId,
					Quantity = bookingService.Quantity,
					TotalPrice = bookingService.Price,
					Name = bookingService.Name
				};
		
				// 3. Remove from local collection
				BookingServices.Remove(tempBookingService);

				await Shell.Current.DisplayAlert("Success", "Service removed from booking", "OK");
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", $"Failed to delete service: {ex.Message}", "OK");
			}
		}

		[RelayCommand]
		private async Task CheckAvailability() 
		{
			SelectedDateTime = SelectedDate + SelectedTime;

			DateTime newBookingEnd;
			//Check if the Selected date time isn't in the past
			if (SelectedDateTime < DateTime.Now)
			{
				await Shell.Current.DisplayAlert("Error", "You Can't Choose Past Date", "OK");
				return;
			}

			//Check if the venue has fixed time or not (To store the right ended time)
			if (SelectedVenue.IsFixedDuration != null && SelectedVenue.IsFixedDuration == true)
			{
				newBookingEnd = EndedDateTime = SelectedDateTime.AddHours((int)SelectedVenue.FixedDurationInHours);
			}

			else
			{

				//Check if the ended time isn't before the started time
				if (EndedTime > SelectedTime)
				{
					newBookingEnd = EndedDateTime = SelectedDate + EndedTime;
				}

				else
				{
					await Shell.Current.DisplayAlert("Warning", "Please Select Appropriate Ending Time", "OK");
					return;
				}
			}


			if (!IsDateTimeAvailable(SelectedDateTime, newBookingEnd))
			{
				await Shell.Current.DisplayAlert("Warning", $"Venue is reserved from {SelectedDateTime} to {newBookingEnd} at this time", "OK");
				return;
			}
			else 
			{
				await Shell.Current.DisplayAlert("Warning", $"Venue is available from {SelectedDateTime} to {newBookingEnd} at this time", "OK");
				return;
			}
		}

		//To add the service for booking
		[RelayCommand]
		private async Task AddSelectedService(ServiceItem service)
		{
		//	await Shell.Current.DisplayAlert("Prompt", "Inside the Add Selected Service Command", "OK");
			
			//await Shell.Current.DisplayAlert("Prompt", $"Service Id {service.ServiceId}\n" +
				//$"Booking Id {SelectedBookingId}\n SelectedQuantity{service.Quantity}", "OK");


			if (service is null || SelectedBookingId == Guid.Empty )
			{
				await Shell.Current.DisplayAlert("Prompt", "Please Choose The Service", "OK");
				return;
			}


			try
			{
				var totalPrice = service.Price * service.Quantity;

				var bookingService = new BookingService
				{
					BookingId = SelectedBookingId,
					ServiceId = service.ServiceId,
					Quantity = service.Quantity,
					TotalPrice = totalPrice,
					Name = service.Name
				};

				BookingServices.Add(bookingService);

				//Insert the Booking Service
				var response = await SupabaseConfig.SupabaseClient
					.From<BookingService>()
					.Insert(bookingService);

				var bookingToUpdate = await SupabaseConfig.SupabaseClient
					.From<Booking>()
					.Where(b => b.BookingId == SelectedBookingId)
					.Get();

				if (bookingToUpdate.Model != null) 
				{
					TotalPrice = bookingToUpdate.Model.TotalPrice += totalPrice;
					await bookingToUpdate.Model.Update<Booking>();
				}

                await Shell.Current.DisplayAlert("Done", "The service has been added to the booking", "OK");
            }
            catch (FileLoadException ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }

        }


        //Method that deals with every user changed for the Date (For SelectedDateTiem)
        partial void OnSelectedDateChanged(DateTime value)
		{
			OnPropertyChanged(nameof(SelectedDateTime));
		}

		//Method that deals with every user changed for the Time (For SelectedDateTiem)
		partial void OnSelectedTimeChanged(TimeSpan value)
		{
			OnPropertyChanged(nameof(SelectedDateTime));
		}

		//Command to load the all bookings that related to that venue and Store them inside the VenueBookings
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
				book.StartDateTime = book.StartDateTime.ToLocalTime();
				book.EndDateTime = book.EndDateTime.ToLocalTime();
				await Shell.Current.DisplayAlert("Prompt", $"{book.StartDateTime.AddHours(-3)} To {book.EndDateTime.AddHours(-3)}", "OK");
			}

		}

		/// <summary>
		/// Check if the selected date and time is available
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		private  bool IsDateTimeAvailable(DateTime newBookingStart, DateTime newBookingEnd)
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
		public async Task ShowTime()
		{
			await Shell.Current.DisplayAlert("Prompt" , $"{SelectedDateTime}" , "OK");
		}


		[RelayCommand]
		public async Task CreateBooking()
		{
			int duration;
			var venue = SelectedVenue;
			DateTime newBookingEnd;

			SelectedDateTime = SelectedDate + SelectedTime;

			//Check if the Selected date time isn't in the past
			if(SelectedDateTime < DateTime.Now) 
			{
				await Shell.Current.DisplayAlert("Error", "You Can't Choose Past Date", "OK");
				return;
			}

			//Check if the venue has fixed time or not (To store the right ended time)
			if (venue.IsFixedDuration != null && venue.IsFixedDuration == true)
			{
				duration = (int)venue.FixedDurationInHours;
				newBookingEnd = EndedDateTime = SelectedDateTime.AddHours(duration);
			}

			else
			{

				//Check if the ended time isn't before the started time
				if (EndedTime > SelectedTime)
				{
					newBookingEnd =  EndedDateTime = SelectedDate + EndedTime;
				}
				
				else
				{
					await Shell.Current.DisplayAlert("Warning", "Please Select Appropriate Ending Time", "OK");
					return;
				}
			}
	
			await Shell.Current.DisplayAlert("Prompt", $"Selected date time value was {SelectedDateTime} To {newBookingEnd}", "OK");

			var newBookingStart = SelectedDateTime.AddHours(3);
			newBookingEnd = newBookingEnd.AddHours(3);

			//Check if the interval is available or not
			if (!IsDateTimeAvailable(newBookingStart, newBookingEnd))
			{
				await Shell.Current.DisplayAlert("Conflict", $"Booking conflict from {newBookingStart.AddHours(-3)} to {newBookingEnd.AddHours(-3)}", "OK");
				return;
			}

			var tempAuth = SupabaseConfig.SupabaseClient.Auth.CurrentUser;

			CurrentUserId = Guid.Parse(tempAuth.Id);

			var newBooking = new Booking
			{
				//BookingId = Guid.NewGuid(),
				UserId = Guid.Parse(tempAuth.Id),
				VenueId = VenueId,
				StartDateTime = newBookingStart,
				EndDateTime = newBookingEnd,
				Status = "confirmed",
				TotalPrice = SelectedVenue.InitialPrice,
				CreatedAt = DateTime.UtcNow.AddHours(3),  // Keep audit fields in UTC
				UpdatedAt = DateTime.UtcNow.AddHours(3)
			};

			TotalPrice = newBooking.TotalPrice;

			//SelectedBookingId = newBooking.BookingId;

			var response = await SupabaseConfig.SupabaseClient.From<Booking>().Insert(newBooking);

			var insertBooking = response.Model;

			SelectedBookingId = insertBooking.BookingId;

			await Shell.Current.DisplayAlert("The Booking Id", $"Booking Id {SelectedBookingId}", "OK");

			if (response.ResponseMessage.IsSuccessStatusCode)
			{
				VenueBookings.Add(newBooking);
				await Shell.Current.DisplayAlert("Success", "Booking created!", "OK");
				await Shell.Current.DisplayAlert("Number of Services", $"{ServicesVenue.Count}", "OK");
				await Shell.Current.GoToAsync(nameof(Pages.ServicesToAdd));
			}
			else
			{
				await Shell.Current.DisplayAlert("Error", "Failed to create booking", "OK");
			}
		}

		// Constructor
		public BookingViewModel()
		{
			
		}

		//Method to deal with parameter that comes from another page
		public async void ApplyQueryAttributes(IDictionary<string, object> query)
		{
			// نحصل على البيانات المرسلة باسم "SelectedVenue"
			if (query.TryGetValue("SelectedVenue", out var venueData))
			{
				SelectedVenue = venueData as Venue;

				if (SelectedVenue != null)
				{
					if (SelectedVenue.IsFixedDuration != null && SelectedVenue.IsFixedDuration == true)
					{
						HasFixedTime = false;
					}
					else
						HasFixedTime = true;

					await LoadHostRules();
					await LoadServices();
					await LoadBookingsAsync(SelectedVenue.VenueId);
					await CheckFavoriteStatusAsync(); // Add this line to check favorite status
				}
			}
		}

		//Method to Load the all host rules that related to the Venue and stored them inside the HostRulesVenue
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

		//Method to load the all Services that related to the Venue and stored them inside the ServicesVenue
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
							Price = serviceId.PricePerUnit, // نأخذ السعر من الجدول الوسيط
							ServiceId = serviceId.ServiceId,
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

		//Command to navigate me to the BookingCalendarPage
		[RelayCommand]
		private async Task GoToBookingCalendarPage() 
		{
			await Shell.Current.GoToAsync(nameof(Pages.BookingCalendarPage));	
		}


		//Method to deals with The Review Page
		[RelayCommand]
		private async Task GoToReviewBooking() 
		{
			await Shell.Current.DisplayAlert("Prompt", $"Date Time {SelectedDateTime}", "OK");
			await Shell.Current.GoToAsync(nameof(Pages.ReviewBooking));			
		}



		[ObservableProperty]
		private bool isFavorite;

		[RelayCommand]
		public async Task ToggleFavorite()
		{
			if (SelectedVenue == null) return;

			try
			{
				var session = SupabaseConfig.SupabaseClient.Auth.CurrentSession;
				if (session == null || session.User == null)
				{
					await Shell.Current.DisplayAlert("Error", "Please login to add favorites", "OK");
					return;
				}

				var userId = Guid.Parse(session.User.Id);

				// Check if venue is already a favorite
				var existingFavorite = await SupabaseConfig.SupabaseClient
					.From<UserFavorite>()
					.Where(f => f.UserId == userId && f.VenueId == SelectedVenue.VenueId)
					.Get();

				if (existingFavorite != null && existingFavorite.Models.Count > 0)
				{
					// If it's a favorite, remove it
					await SupabaseConfig.SupabaseClient
						.From<UserFavorite>()
						.Where(f => f.UserId == userId && f.VenueId == SelectedVenue.VenueId)
						.Delete();

					IsFavorite = false;
					await Shell.Current.DisplayAlert("Success", "Removed from favorites", "OK");
				}
				else
				{
					// If not a favorite, add it
					var newFavorite = new UserFavorite
					{
						UserId = userId,
						VenueId = SelectedVenue.VenueId,
						CreatedAt = DateTime.UtcNow
					};

					await SupabaseConfig.SupabaseClient
						.From<UserFavorite>()
						.Insert(newFavorite);

					IsFavorite = true;
					await Shell.Current.DisplayAlert("Success", "Added to favorites", "OK");
				}
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", $"Failed to update favorites: {ex.Message}", "OK");
			}
		}

		// Method to check if this venue is already a favorite
		private async Task CheckFavoriteStatusAsync()
		{
			if (SelectedVenue == null) return;

			try
			{
				var session = SupabaseConfig.SupabaseClient.Auth.CurrentSession;
				if (session == null || session.User == null)
				{
					IsFavorite = false;
					return;
				}

				var userId = Guid.Parse(session.User.Id);

				var existingFavorite = await SupabaseConfig.SupabaseClient
					.From<UserFavorite>()
					.Where(f => f.UserId == userId && f.VenueId == SelectedVenue.VenueId)
					.Get();

				IsFavorite = existingFavorite != null && existingFavorite.Models.Count > 0;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error checking favorite status: {ex.Message}");
				IsFavorite = false;
			}
		}

		 [RelayCommand]
		  private async Task HomeClicked()
        {
            // Navigate to HomePage 
           await Shell.Current.GoToAsync("//Home");
        }
	}
}