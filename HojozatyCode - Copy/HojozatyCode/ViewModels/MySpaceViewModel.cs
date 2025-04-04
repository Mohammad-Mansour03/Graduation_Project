using HojozatyCode.Models;
using HojozatyCode.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Maui.Controls;
using System;

namespace HojozatyCode.ViewModels
{
    public partial class MySpaceViewModel : ObservableObject
    {

        [RelayCommand]
        private async Task GoToAddSpace()
        {
            await Shell.Current.GoToAsync(nameof(Pages.SpaceTypeSelectionPage));
        }

        [ObservableProperty]
        private ObservableCollection<Venue> venues ;

        [ObservableProperty]
        private bool isLoading = false;

        public MySpaceViewModel()
        {
           //// LoadUserVenuesCommand = new AsyncRelayCommand(LoadUserVenuesAsync);
           // _ = LoadUserVenuesAsync();

            Venues = new ObservableCollection<Venue>();
        }

        //public IAsyncRelayCommand LoadUserVenuesCommand { get; }

        public async Task LoadVenuesAsync()
        {
            if (SupabaseConfig.SupabaseClient == null)
                await SupabaseConfig.InitializeAsync();

           // IsLoading = true;

            try
            {
                // Assuming you have a way to get the current logged-in user's ID
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
                    .Where(v => v.OwnerId == userIdGuid )
                    .Get();

				Venues.Clear();

				foreach (var venue in response.Models)
				{
					Venues.Add(venue);
				}
			}
            catch (Exception ex)
            {
                // Handle the error (log or show message to user)
               await Shell.Current.DisplayAlert("Error",$"Error loading venues: {ex.Message}","OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
