using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using Supabase;
using Supabase.Postgrest;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Threading.Tasks;

namespace HojozatyCode.ViewModels
{
	public partial class EditProfileViewModel : ObservableObject
	{
		[ObservableProperty]
		private string firstName;

		[ObservableProperty]
		private string lastName;

		[ObservableProperty]
		private string email;

		[ObservableProperty]
		private string phoneNumber;

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
					LastName = response.LastNameC;
					Email = response.EmailC;
					PhoneNumber = response.PhoneC;
				}
			}

		}

	}
}