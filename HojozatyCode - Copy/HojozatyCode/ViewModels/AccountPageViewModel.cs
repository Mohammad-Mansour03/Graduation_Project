using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Services;
using HojozatyCode.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HojozatyCode.ViewModels
{
    public partial class AccountPageViewModel : ObservableObject
    {

        //(Here Was The Big Problem)
        //Method to LogOut the user from the application
		[RelayCommand]
		private async Task Logout()
		{
			try
			{
                await SupabaseConfig.SupabaseClient.Auth.SignOut();

				// Navigate back to the login page
				await Shell.Current.GoToAsync(nameof(Pages.LogInPage));
			}
			catch (Exception ex)
			{
				// Optional: show error to user
				await Shell.Current.DisplayAlert("Logout Failed", ex.Message, "OK");
			}
		}


		//Navigte me to the Edit Profile Page
		[RelayCommand]
        private async Task GoToEditProfile() 
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(Pages.EditProfile));
            }
            catch (Exception ex) 
            {
                await Shell.Current.DisplayAlert("Error", $"{ex.Message}", "OK");
            }
        }


        //Navigate me to the Home Page
        [RelayCommand]  
        private async Task GoBack()
        {
            try
            {
                await Shell.Current.GoToAsync("//Home");
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", $"{ex.Message}", "OK");
			}
		}

        //Navigate me to the Favourites Page
        [RelayCommand]
        private async Task GoToFavoutrites()
        {
            try
            {
                await Shell.Current.GoToAsync("//Favourite");
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", $"{ex.Message}", "OK");
			}
		}

        //Navigate me to the Add Space Page
        [RelayCommand]
        private async Task GoToAddSpace()
        {
            try
            {
                await Shell.Current.GoToAsync("//AddSpace");
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", $"{ex.Message}", "OK");
			}
		}

        //Navigate me to the My Space Page
        [RelayCommand]  
        private async Task GoToMySpace()
        {
            try
            {
                await Shell.Current.GoToAsync("//MySpace");
            }
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", $"{ex.Message}", "OK");
			}
		}

    }

    
}
