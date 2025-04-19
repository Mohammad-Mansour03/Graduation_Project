using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HojozatyCode.ViewModels
{
    public partial class BookingViewModel : ObservableObject
    {
        // Singleton instance
        private static BookingViewModel _instance;
        public static BookingViewModel Instance => _instance ??= new BookingViewModel();

        // Selected venue for booking
        [ObservableProperty]
        private Venue selectedVenue;

        // Booking details
        [ObservableProperty]
        private DateTime startDateTime = DateTime.Now.AddHours(1);

        [ObservableProperty]
        private DateTime endDateTime = DateTime.Now.AddHours(3);

        [ObservableProperty]
        private double totalPrice;

        [ObservableProperty]
        private string status = "Pending";

        // UI state properties
        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string errorMessage;

        // Constructor - private to enforce singleton pattern
        private BookingViewModel()
        {
            // Initialize if needed
        }

        // Load venue details by ID
        [RelayCommand]
        public async Task LoadVenueAsync(Guid venueId)
        {
            if (venueId == Guid.Empty)
                return;

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var response = await SupabaseConfig.SupabaseClient
                    .From<Venue>()
                    .Where(v => v.VenueId == venueId)
                    .Get();

                if (response?.Models?.Count > 0)
                {
                    SelectedVenue = response.Models[0];
                    
                    // Set the initial price from venue
                    TotalPrice = SelectedVenue.InitialPrice;
                }
                else
                {
                    ErrorMessage = "Venue not found";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading venue: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        // // Create booking
        // [RelayCommand]
        // public async Task CreateBookingAsync()
        // {
        //     if (SelectedVenue == null)
        //     {
        //         ErrorMessage = "No venue selected";
        //         return;
        //     }

        //     try
        //     {
        //         IsLoading = true;
        //         ErrorMessage = string.Empty;

        //         // Get current user ID
        //         var session = SupabaseConfig.SupabaseClient.Auth.CurrentSession;
        //         if (session == null || session.User == null)
        //         {
        //             ErrorMessage = "Please log in to book a venue";
        //             return;
        //         }

        //         var userId = Guid.Parse(session.User.Id);

        //         // Create the booking record
        //         var booking = new Booking
        //         {
        //             BookingId = Guid.NewGuid(),
        //             UserId = userId,
        //             VenueId = SelectedVenue.VenueId,
        //             StartDateTime = StartDateTime,
        //             EndDateTime = EndDateTime,
        //             TotalPrice = TotalPrice,
        //             Status = Status,
        //             CreatedAt = DateTime.Now,
        //             UpdatedAt = DateTime.Now
        //         };

        //         var response = await SupabaseConfig.SupabaseClient
        //             .From<Booking>()
        //             .Insert(booking);

        //         if (response != null && response.Models.Count > 0)
        //         {
        //             // Booking created successfully
        //             await Shell.Current.DisplayAlert("Success", "Your booking has been created!", "OK");
                    
        //             // Navigate back or to confirmation page
        //             await Shell.Current.GoToAsync("..");
        //         }
        //         else
        //         {
        //             ErrorMessage = "Failed to create booking";
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         ErrorMessage = $"Error creating booking: {ex.Message}";
        //     }
        //     finally
        //     {
        //         IsLoading = false;
        //     }
        // }

        // // Reset booking state
        // public void ResetBooking()
        // {
        //     SelectedVenue = null;
        //     StartDateTime = DateTime.Now.AddHours(1);
        //     EndDateTime = DateTime.Now.AddHours(3);
        //     TotalPrice = 0;
        //     Status = "Pending";
        //     ErrorMessage = string.Empty;
        // }
    }
}