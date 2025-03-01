using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HojozatyCode.Pages;

namespace HojozatyCode.ViewModels
{
    public partial class AddSpaceViewModel : ObservableObject
    {
        // Automatically generated properties using [ObservableProperty]
        [ObservableProperty]
        private string ownerName;

        [ObservableProperty]
        private string spaceName;

        [ObservableProperty]
        private string city;

        [ObservableProperty]
        private string address;

        [ObservableProperty]
        private string category; // Changed from SpaceType to Category

        [ObservableProperty]
        private string spaceType;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
         private string email;

        [ObservableProperty]
         private string phone;

        [ObservableProperty]
         private int capacity;

         [ObservableProperty]
         private double initialPrice;

        [ObservableProperty]
        private ObservableCollection<FileResult> selectedImages;

        [ObservableProperty]
        private ObservableCollection<ImageSource> imagePreviewSources;

        [ObservableProperty]
        private bool isLoading;

        // Constructor to initialize the collections and set up initial values
        public AddSpaceViewModel()
        {
            SelectedImages = new ObservableCollection<FileResult>();
            ImagePreviewSources = new ObservableCollection<ImageSource>();

            // Initialize with 9 empty slots for images
            for (int i = 0; i < 9; i++)
            {
                SelectedImages.Add(null);
                ImagePreviewSources.Add(null);
            }
        }

        #region Navigation Commands

        // Command to navigate to the SpaceInformationPage
        [RelayCommand]
        private async Task NavigateToSpaceInformationAsync()
        {
            // Validate if Category is selected
            if (string.IsNullOrWhiteSpace(SpaceType))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Please select a category.", "OK");
                return;
            }

            // Navigate to SpaceInformationPage
            await Shell.Current.GoToAsync(nameof(SpaceInformationPage));
        }

        // Command to navigate to the ReviewPage
        [RelayCommand]
        private async Task NavigateToReviewPageAsync() =>
            await Shell.Current.GoToAsync(nameof(ReviewPage));

        // Command to navigate to the SpacePoliciesPage
        [RelayCommand]
        private async Task NavigateToSpacePoliciesPageAsync() =>
            await Shell.Current.GoToAsync(nameof(SpacePolicies));

        // Command to navigate to the SuccessPage
        [RelayCommand]
        private async Task NavigateToSuccessPageAsync() =>
            await Shell.Current.GoToAsync(nameof(SuccessPage));

        // Command to navigate to the SpacePicturesPage
        [RelayCommand]
        private async Task NavigateToSpacePicturesAsync() =>
            await Shell.Current.GoToAsync(nameof(SpacePicturesPage));

        // Command to navigate to the HomePage
        [RelayCommand]
        private async Task GoToHomeAsync() =>
            await Shell.Current.GoToAsync(nameof(HomePage));

        // Command to navigate to the ServicesPage
        [RelayCommand]
        private async Task NavigateToServicesPageAsync()
        {
            // Validate if at least one image is selected
            if (!SelectedImages.Any(x => x != null))
            {
                await Shell.Current.DisplayAlert("Validation", "Please select at least one image.", "OK");
                return;
            }

            // Navigate to ServicesPage
            await Shell.Current.GoToAsync(nameof(ServicesPage));
        }

        #endregion

        #region Venue Save & Image Commands

        // Command to save the space (venue) information
        [RelayCommand]
        private async Task SaveSpaceAsync()
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(SpaceName) ||
                string.IsNullOrWhiteSpace(Description) ||
                string.IsNullOrWhiteSpace(Category) || // Changed from SpaceType to Category
                string.IsNullOrWhiteSpace(City) ||
                string.IsNullOrWhiteSpace(Address))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Please fill in all required fields.", "OK");
                return;
            }

            // Collect non-null images
            var imagesToUpload = SelectedImages.Where(img => img != null).ToList();
            if (!imagesToUpload.Any())
            {
                await Shell.Current.DisplayAlert("Warning", "Please select at least one image.", "OK");
                return;
            }

            try
            {
                IsLoading = true;
                await Shell.Current.DisplayAlert("Processing", "Saving your venue and uploading images. Please wait...", "OK");

                // Create a new venue object
                var venue = new Venue
                {
                    VenueId = Guid.NewGuid(),
                    OwnerId = Guid.Parse(SupabaseConfig.SupabaseClient.Auth.CurrentUser.Id),
                    VenueName = SpaceName,
                    Description = Description,
                    Type = SpaceType, // Changed from SpaceType to Category
                    Capacity = Capacity, // the number may be stored wrong in the database
                    Location = $"{City}, {Address}",
                    VenueContactPhone = Phone, // Example value
                    VenueEmail = Email, // Example value
                    InitialPrice = InitialPrice // the number may be stored wrong in the database
                };

                Console.WriteLine($"Creating venue with ID: {venue.VenueId}");

                // Save venue with uploaded images and category
                bool success = await VenueService.CreateVenueAsync(venue, imagesToUpload, Category, Description);
                if (success)
                {
                    await Shell.Current.DisplayAlert("Success", "Venue and category created successfully!", "OK");
                    await Shell.Current.GoToAsync(nameof(SuccessPage));
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to create venue.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving venue: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // Command to add an image to the selected images collection
        [RelayCommand]
        private async Task AddImageAsync(string index)
        {
            try
            {
                IsLoading = true;
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select Image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    if (int.TryParse(index, out int idx) && idx >= 0 && idx < SelectedImages.Count)
                    {
                        SelectedImages[idx] = result;

                        // Create an image preview
                        var stream = await result.OpenReadAsync();
                        ImagePreviewSources[idx] = ImageSource.FromStream(() => stream);
                        OnPropertyChanged(nameof(ImagePreviewSources));
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Invalid image index.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        #region Helper Methods

        // Converts a FileResult to an ImageSource
        private async Task<ImageSource> ConvertToImageSourceAsync(FileResult file)
        {
            if (file == null)
                return null;

            var stream = await file.OpenReadAsync();
            return ImageSource.FromStream(() => stream);
        }

        #endregion
    }
}