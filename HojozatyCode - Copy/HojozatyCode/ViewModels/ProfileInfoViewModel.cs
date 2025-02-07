using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using HojozatyCode.Models;


namespace HojozatyCode.ViewModels
{
    public partial class ProfileInfoViewModel : ObservableObject
    {
		[ObservableProperty]
		private User user = new User();


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
            await Shell.Current.GoToAsync(nameof(Pages.HomePage));
        }

	}
}