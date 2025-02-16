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

namespace HojozatyCode.ViewModels
{
	public partial class EditProfileViewModel : ObservableObject
	{
	
		//Properety to store the user first name
		[ObservableProperty]
		private string firstName;

		//Properety to store the user last name
		[ObservableProperty]
		private string lastName;
		
		//Properety to store the user middle name
		[ObservableProperty]
		private string middleName;

		//Properety to store the user email
		[ObservableProperty]
		private string email;

		//Properety to sotre the user phone number
		[ObservableProperty]
		private string phoneNumber;

		//Properety to sotre the user age
		[ObservableProperty]
		private int age;

		//Properety to sotre the user gender
		[ObservableProperty]
		private string gender;
		
		//Properety to sotre the error
		[ObservableProperty]
		private string errorMessage;

		public EditProfileViewModel()
		{
			LoadUserData();
		}

		private async void LoadUserData()
		{
			var client = SupabaseConfig.SupabaseClient;

			var userId = client.Auth.CurrentUser?.Id;

			if (userId != null)
			{
				var response = await client.From<User>()
										   .Filter("user_id", Supabase.Postgrest.Constants.Operator.Equals, userId)
										   .Single();

				if (response != null)
				{
					FirstName = response.FirstNameC;
					MiddleName = response.MiddleNameC;
					LastName = response.LastNameC;
					Email = response.EmailC;
					PhoneNumber = response.PhoneC;
					Age = response.AgeC;
					Gender = response.GenderC;
				};
			}

		}//The end of Load User Command

		[RelayCommand]
		private async Task SaveUserData() 
		{
			try
			{
				
				var client = SupabaseConfig.SupabaseClient;

				var userId = client.Auth.CurrentUser?.Id;

				if (string.IsNullOrEmpty(userId))
				{
					await Shell.Current.DisplayAlert("Error", "User not found.", "OK");
					return;
				}

				if (!IsValidEmail(Email)) 
				{
					await Shell.Current.DisplayAlert("Error", "Please Enter Valid Email", "Ok");
					return;
				}

				var updatedUser = new User
				{
					UserIdC = Guid.Parse(userId),
					FirstNameC = FirstName,
					MiddleNameC = MiddleName,
					LastNameC = LastName,
					EmailC = Email,
					PhoneC = PhoneNumber,
					AgeC = Age,
					GenderC = Gender
				};

				var response = await client.From<User>()
										   .Filter("user_id", Supabase.Postgrest.Constants.Operator.Equals, userId)
										   .Update(updatedUser);

				if (response != null)
				{
					await Shell.Current.DisplayAlert("Success", "Profile updated successfully!", "OK");
				}
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", $"Failed to update profile: {ex.Message}", "OK");
			}

		}

		private bool IsValidEmail(string email) 
		{
			return  Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z.-]+\.[a-zA-Z]{2,}$");
		}


		[RelayCommand]
		private async Task CloseEditProfile() 
		{
			await Shell.Current.GoToAsync(nameof(Pages.HomePage));
		}
	}
}