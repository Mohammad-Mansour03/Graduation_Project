using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;


namespace HojozatyCode.ViewModels
{
	
	public partial class BookingViewModel : ObservableObject , IQueryAttributable
	{
        // Singleton instance
        private static BookingViewModel _instance;

		[ObservableProperty]
		private string venueIdRaw;

		[ObservableProperty]
		private Guid venueId;

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
        public BookingViewModel()
        {
            // Initialize if needed
        }

		public void ApplyQueryAttributes(IDictionary<string, object> query)
		{
			// نحصل على البيانات المرسلة باسم "SelectedVenue"
			if (query.TryGetValue("SelectedVenue", out var venueData))
			{
				SelectedVenue = venueData as Venue;
			}
		}


	
	}
}