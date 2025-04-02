using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HojozatyCode.ViewModels
{
    public partial class AccountPageViewModel : ObservableObject
    {
        //Navigte me to the Edit Profile Page
        [RelayCommand]
        private async Task GoToEditProfile() 
        {
            await Shell.Current.GoToAsync(nameof(Pages.EditProfile));
        }

        [RelayCommand]
        private async Task GoToAdminPanel()
        {
            await Shell.Current.GoToAsync(nameof(Pages.AdminPanel));
        }

        //Navigate me to the Home Page
        [RelayCommand]  
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync(nameof(Pages.HomePage));
        }    
        
        //Navigate me to the My Space Page
        [RelayCommand]  
        private async Task GoToMySpace()
        {
            await Shell.Current.GoToAsync(nameof(Pages.MySpace));
        }
        private async Task GoToAddSpace()
        {
            await Shell.Current.GoToAsync(nameof(Pages.SpaceTypeSelectionPage));
        }
    }

    
}
