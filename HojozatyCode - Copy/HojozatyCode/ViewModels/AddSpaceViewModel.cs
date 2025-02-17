using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HojozatyCode.Pages;
using Microsoft.Maui.Storage;

namespace HojozatyCode.ViewModels
{
    public partial class AddSpaceViewModel : ObservableObject
    {
        [ObservableProperty]
        private string ownerName;

        [ObservableProperty]
        private string spaceName;

        [ObservableProperty]
        private string city;

        [ObservableProperty]
        private string address;

        [ObservableProperty]
        private string spaceType;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private ObservableCollection<string> selectedImages;

        public AddSpaceViewModel()
        {
            SelectedImages = new ObservableCollection<string>();
            // Initialize with 9 null slots
            for (int i = 0; i < 9; i++)
            {
                SelectedImages.Add(null);
            }
        }

        [RelayCommand]
        private async Task NavigateToSpaceInformation()
        {
            try
            {
                if (string.IsNullOrEmpty(SpaceType))
                {
                    await Shell.Current.DisplayAlert("Validation Error", "Please select a space type", "OK");
                    return;
                }
            
                await Shell.Current.GoToAsync(nameof(SpaceInformationPage));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Navigation Error", ex.Message, "OK");
                Console.WriteLine($"Navigation Error: {ex.Message}");
            }
        }


        [RelayCommand]
        private async Task NavigateToReviewPage()
        {
            await Shell.Current.GoToAsync(nameof(ReviewPage));
        }

        [RelayCommand]
        private async Task NavigateToSpacePoliciesPage()
        {
            await Shell.Current.GoToAsync(nameof(SpacePolicies));
        }

        [RelayCommand]
        private async Task NavigateToSuccessPage()
        {
            await Shell.Current.GoToAsync(nameof(SuccessPage));
        }

        [RelayCommand]
        private async Task NavigateToSpacePictures()
        {
            await Shell.Current.GoToAsync(nameof(SpacePicturesPage));
        }

        [RelayCommand]
        private async Task SaveSpace()
        {
            // Add logic to save space information
            await Shell.Current.DisplayAlert("Success", "Space information saved successfully.", "OK");
        }

        [RelayCommand]
        private async Task GoToHome()
        {
            await Shell.Current.GoToAsync(nameof(HomePage));
        }

        [RelayCommand]
        private async Task NavigateToServicesPage()
        {
            if (!SelectedImages.Any(x => x != null))
            {
                await Shell.Current.DisplayAlert("Validation", "Please select at least one image", "OK");
                return;
            }

            await Shell.Current.GoToAsync(nameof(Pages.ServicesPage));
        }

        [RelayCommand]
        private async Task AddImage(string index)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select Image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    var idx = int.Parse(index);
                    SelectedImages[idx] = result.FullPath;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}