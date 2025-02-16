using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HojozatyCode.Pages;
using Microsoft.Maui.Storage;

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
        await Shell.Current.GoToAsync(nameof(ServicesPage));
    }

    [RelayCommand]
    private async Task AddImage()
    {
        try
        {
            var options = new PickOptions
            {
                PickerTitle = "Select Image",
                FileTypes = FilePickerFileType.Images
            };

            var result = await FilePicker.PickAsync(options);
            if (result != null)
            {
                if (SelectedImages.Count >= 9)
                {
                    await Shell.Current.DisplayAlert("Limit Reached", "You can only add up to 9 images", "OK");
                    return;
                }

                SelectedImages.Add(result.FullPath);
                await Shell.Current.DisplayAlert("Success", "Image added successfully", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Failed to add image: " + ex.Message, "OK");
        }
    }
}