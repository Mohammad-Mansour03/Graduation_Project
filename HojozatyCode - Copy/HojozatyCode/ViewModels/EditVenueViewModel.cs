using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using Supabase;
using Supabase.Postgrest;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HojozatyCode.ViewModels
{

	public partial class EditVenueViewModel : ObservableObject , IQueryAttributable
	{

		[ObservableProperty]
		private Venue venue;

		//Properety to store the venue name
		[ObservableProperty]
		private string venueName;	
		
		//Properety to store the venue description
		[ObservableProperty]
		private string description;
		
		//Properety to store the venue type
		[ObservableProperty]
		private string type;
		
		//Properety to store the venue location
		[ObservableProperty]
		private string location;

		//Properety to store the venue phone
		[ObservableProperty]
		private string venuePhone;

		//Properety to store the venue email
		[ObservableProperty]
		private string venueEmail;

		//Properety to sotre the venue capacity
		[ObservableProperty]
		private int venueCapacity;
		
		//Properety to sotre the venue capacity
		[ObservableProperty]
		private double initialPrice;
		
		//Properety to sotre the venue cancellation policy
		[ObservableProperty]
		private string cancellationPolicy;
		
		//Properety to sotre the venue city 
		[ObservableProperty]
		private string city;	
		
		//Properety to sotre the venue Image 
		[ObservableProperty]
		private string imageUrl;

		//Property to store the ErrorMessage
		[ObservableProperty]
		private string errorMessage;

		public void ApplyQueryAttributes(IDictionary<string, object> query)
		{
			if (query.TryGetValue("VenueToEdit", out var venueObj) && venueObj is Venue venueToEdit)
			{
				Venue = venueToEdit;

				// Copy the Values inside my properties
				VenueName = Venue.VenueName;
				VenuePhone = Venue.VenueContactPhone;
				VenueEmail = Venue.VenueEmail;
				VenueCapacity = Venue.Capacity;
				InitialPrice = Venue.InitialPrice;
				ImageUrl = Venue.ImageUrls[0];
			}
		}

		//This Method to Save the new venue data
		[RelayCommand]
		private async Task SaveVenue()
		{
			var client = SupabaseConfig.SupabaseClient;

			try
			{
				//await Shell.Current.DisplayAlert("Prompt", "Image URL before update: " + Venue.ImageUrl, "OK");

				if (!IsValidEmail(VenueEmail))
				{
					ErrorMessage ="Please Enter Valid Email";
					return;
				}

				if (!IsValidPhone(VenuePhone)) 
				{
					ErrorMessage = "Please Enter Valid Phone";
					return;
				}

				var updatedVenue = new Venue
				{
					VenueId = Venue.VenueId,
					OwnerId = Venue.OwnerId,
					VenueName = VenueName,
					ImageUrl = Venue.ImageUrl,
					VenueEmail = VenueEmail,
					VenueContactPhone = VenuePhone,
					Capacity = Venue.Capacity,
					InitialPrice = InitialPrice,
					Description = Venue.Description,
					Type = Venue.Type,
					Location = Venue.Location,
					CancellationPolicy = Venue.CancellationPolicy,
					City = Venue.City,
				};

				var response = await client.From<Venue>()
										   .Where(v => v.VenueId == Venue.VenueId)
										   .Update(updatedVenue);

				if (response != null)
				{
					await Shell.Current.DisplayAlert("Success", "Profile updated successfully!", "OK");
					await Shell.Current.GoToAsync("//MySpace");
				}

			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", $"Failed to update profile: {ex.Message}", "OK");
			}

		}

		//Method to validate the email
		private bool IsValidEmail(string email)
		{
			return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z.-]+\.[a-zA-Z]{2,}$");
		}
			
		//Method to validate the Phone
		private bool IsValidPhone(string phone)
		{
			return Regex.IsMatch(phone, @"^(?:\+962|0)7[789]\d{7}$");
		}

		[RelayCommand]
		public async Task DeleteVenueEdit(Venue venue)
		{
			var client = SupabaseConfig.SupabaseClient;

			bool isConfirmed = await Shell.Current.DisplayAlert(
			  "Delete Confirmation",
			 $"Are your sure you want to delete : {venue.VenueName}",
			  "Yes", "No");

			if (!isConfirmed)
				return;

			try
			{

				await SupabaseConfig.SupabaseClient
							   .From<Venue>()
							   .Where(v => v.VenueId == venue.VenueId)
							   .Delete();

				await Shell.Current.DisplayAlert("Done", $"Your Venue Deleted Successfully", "OK");

				await Shell.Current.GoToAsync("//MySpace");	

			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", $"Failed to delete venue: {ex.Message}", "OK");
			}
		}

		//Method to navigate to the my space page when click on the cancel button
		[RelayCommand]
		private async Task CloseEdit()
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