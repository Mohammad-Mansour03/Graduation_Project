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
        [ObservableProperty]
        private ObservableCollection<Venue> userVenues = new();

        [ObservableProperty]
        private bool isLoading = false;

        public MySpaceViewModel()
        {
            LoadUserVenuesCommand = new AsyncRelayCommand(LoadUserVenuesAsync);
        }

        public IAsyncRelayCommand LoadUserVenuesCommand { get; }

        private async Task LoadUserVenuesAsync()
        {
            if (SupabaseConfig.SupabaseClient == null)
                await SupabaseConfig.InitializeAsync();

            IsLoading = true;

            try
            {
                // Assuming you have a way to get the current logged-in user's ID
                var session = SupabaseConfig.SupabaseClient.Auth.CurrentSession;
                if (session == null || session.User == null)
                {
                    IsLoading = false;
                    return;
                }
                
                var userId = session.User.Id;

                var venues = await SupabaseConfig.SupabaseClient
                    .From<Venue>()
                    .Select("*")
                    .Where(v => v.OwnerId.Equals(userId))
                    .Get();

                if (venues != null && venues.Models.Any())
                {
                    UserVenues = new ObservableCollection<Venue>(venues.Models);
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
