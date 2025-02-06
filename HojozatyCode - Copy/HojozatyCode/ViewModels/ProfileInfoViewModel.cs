using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace HojozatyCode.ViewModels
{
    public partial class ProfileInfoViewModel : ObservableObject
    {
        //Profile Information Properities
        [ObservableProperty]
        private string firstName;
        
        [ObservableProperty]
        private string middleName;
        
        [ObservableProperty]
        private string lastName;
         
        [ObservableProperty]
        private string phoneNumber;
        
        [ObservableProperty]
        private short ?age ;
        
        [ObservableProperty]
        private bool isMale = true;//Default to Male
        
        [ObservableProperty]
        private bool isFemale;


		//Handle Changes in gender selection
		partial void OnIsMaleChanged(bool value)
		{
			if (value)
                IsFemale = false; // If male is selected, unselect female
		}

		partial void OnIsFemaleChanged(bool value)
		{
            if (value)
                IsMale = false;//if female is selected , unselect male
		}

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