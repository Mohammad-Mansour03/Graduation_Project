using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using System.Collections.ObjectModel;
using HojozatyCode.Pages;
using HojozatyCode.Services;
namespace HojozatyCode.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        //Collection to store some venues to display them inside the Home Page
        [ObservableProperty]
        ObservableCollection<Venue> homeVenues = new ();

        private bool _hasLoaded = false;
		public async Task LoadVenues()
		{
            if (_hasLoaded)
                return;

            _hasLoaded = true;

			string[] types = { "Wedding", "Meeting", "Funeral", "Photography", "Cultural Events", "Entertainment", "Sports" };


			foreach (var type in types)
			{
				var result = await SupabaseConfig.SupabaseClient
					.From<Venue>()
					.Filter("status", Supabase.Postgrest.Constants.Operator.Equals, "Approved")
			        .Filter("type", Supabase.Postgrest.Constants.Operator.ILike, $"%{type}%")
					.Get();

				var venue = result.Models.FirstOrDefault();

				if (venue != null && !HomeVenues.Any(v => v.VenueId == venue.VenueId))
				{
					HomeVenues.Add(venue);
				}
			}
		}

        //Navigate to Choosing Hall Booking with passing the right Venue
		[RelayCommand]
		public async Task VenueSelectedAsync(Venue selectedVenue)
		{
			if (selectedVenue == null) 
                return;

			var navParams = new Dictionary<string, object>
			{
				{ "SelectedVenue", selectedVenue }
			};

			await Shell.Current.GoToAsync(nameof(Pages.ChoosingHallBooking), navParams);
		}

		//Command for back icon (Navigate me to the Login and Signup page)
		[RelayCommand]
        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync(nameof(Pages.LoginSignupPage));
        }

       
        //Navigate me to the Account Page
        [RelayCommand]
        private async Task GoToAccount()
        {
            await Shell.Current.GoToAsync(nameof(Pages.AccountPage));
        }


        // Navigate to notification
        [RelayCommand]
        private async Task NavigateToNotifications()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(Pages.NotificationsPage));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Navigation Error", ex.Message, "OK");
                return;
            }
        }
       
        //Navigate to the Categoty Venue page with passing the right category
        [RelayCommand]
        private async Task NavigateToCategory(string category)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "Category", category }
                   
                };

                await Shell.Current.GoToAsync($"{nameof(CategoryVenuesPage)}", true, parameters);
            }

            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Navigation Error", ex.Message, "OK");
                Console.WriteLine($"Navigation Error: {ex.Message}");
            }
        }

    }
}
