using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using HojozatyCode.Models;
using HojozatyCode.Services;


namespace HojozatyCode.ViewModels
{
    public partial class ProfileInfoViewModel : ObservableObject
    {
		[ObservableProperty]
		string firstNameP;

		[ObservableProperty]
		string middleNameP;
		
		[ObservableProperty]
		string lastNameP;
		
		[ObservableProperty]
		string phoneP;

		[ObservableProperty]
		int ageP;

		[ObservableProperty]
		string genderP;



		//Handle Changes in gender selection
		//partial void OnIsMaleChanged(bool value)
		//{
		//	if (value)
		//              IsFemale = false; // If male is selected, unselect female
		//}

		//partial void OnIsFemaleChanged(bool value)
		//{
		//          if (value)
		//              IsMale = false;//if female is selected , unselect male
		//}

		[RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync(nameof(Pages.SignUpPage));
        }

        [RelayCommand]
        private async Task NextAsync()
        {
			try
			{
				var authUser = SupabaseConfig.SupabaseClient.Auth.CurrentUser;
				if (authUser == null)
				{
					await Shell.Current.DisplayAlert("Error", "User not authenticated.", "OK");
					return;
				}

				var newUserProfile = new User
				{
					UserIdC = Guid.Parse(authUser.Id),
					FirstNameC = FirstNameP,
					MiddleNameC = MiddleNameP,
					LastNameC = LastNameP,
					EmailC = authUser.Email,
					PhoneC = PhoneP,
					AgeC = AgeP,
					GenderC = GenderP,
				};

				var response = await SupabaseConfig.SupabaseClient
					.From<User>()
					.Insert(newUserProfile);

				if (response != null)
				{
					await Shell.Current.GoToAsync(nameof(Pages.HomePage));  // Navigate to HomePage after signup
				}
				else
				{
					await Shell.Current.DisplayAlert("Error", "Failed to save profile. Try again.", "OK");
				}
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", $"Failed to save profile: {ex.Message}", "OK");
			}
		}

	}
}