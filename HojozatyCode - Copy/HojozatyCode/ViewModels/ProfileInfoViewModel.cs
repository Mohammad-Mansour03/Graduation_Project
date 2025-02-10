using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using HojozatyCode.Models;
using HojozatyCode.Services;


namespace HojozatyCode.ViewModels
{
    public partial class ProfileInfoViewModel : ObservableObject
    {
		//The User first Name properety
		[ObservableProperty]
		string firstNameP;

		//The User Middle Name properety
		[ObservableProperty]
		string middleNameP;

		//The User Last Name properety
		[ObservableProperty]
		string lastNameP;

		//The User Phone Number properety
		[ObservableProperty]
		string phoneP;

		//The User Age properety
		[ObservableProperty]
		int ageP;

		//The User Gender properety
		[ObservableProperty]
		string genderP;

		//The command to Go out from the profile info page
		[RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync(nameof(Pages.SignUpPage));
        }
		
		//The Commant to sotre the user information in the Profile table in database 
        [RelayCommand]
        private async Task NextAsync()
        {
			try
			{
				//To retrieve the Id and the User's email 
				var authUser = SupabaseConfig.SupabaseClient.Auth.CurrentUser;
				
				if (authUser == null)
				{
					await Shell.Current.DisplayAlert("Error", "User not authenticated.", "OK");
					return;
				}

				//Make the user object to store the user information and saved it in the database
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

				//Add the user to the Profile table in the database
				var response = await SupabaseConfig.SupabaseClient
					.From<User>()
					.Insert(newUserProfile);

				//Navigate to the home page if the user was stored in the database
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