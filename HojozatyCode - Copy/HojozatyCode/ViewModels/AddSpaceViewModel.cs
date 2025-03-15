using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HojozatyCode.Pages;
using static Microsoft.Maui.ApplicationModel.Permissions;
using System.Text.RegularExpressions;
using Supabase;

namespace HojozatyCode.ViewModels
{
    public partial class AddSpaceViewModel : ObservableObject
    {
        private readonly Client _supabaseClient;

        //(SpaceSelcetion Page)
		//Collection Properety to sotre Collection of spaces types the user
		//Can choose for one space
		[ObservableProperty]
		private ObservableCollection<string> selectedSpaceTypes;

		// Properety to sotre the all spaces type in specific format inside 
		//the database table (type1, type2, ...)
		[ObservableProperty]
		private string spaceType;

		//(Space Information Page)
        //Dictionary to sotore the SpaceTypes for every space category
		private readonly Dictionary<string, List<string>> SpaceTypeCategories = new()
{
	{ "Wedding", new List<string> { "Halls", "Farms", "Hotels", "Outdoors" } },
	{ "Entertainment", new List<string> { "Farms", "Adventure Spots", "WorkShops" } },
	{ "Work/Meeting Space", new List<string> { "ClassRooms/Office Spaces", "Farms", "Outdoor Space" , "Majls" } },
	{ "Funeral", new List<string> { "Diwan", "Dedicated Funeral Halls" } },
	{ "Photography", new List<string> { "Photography Studios", "Outdoor Photography Spaces" , "Product Photography Spaces" } },
	{ "Sports", new List<string> { "Staduim" } },
	{ "Cultural Events", new List<string> { "Farms", "Majls", "Cultrual Evening Venues" , "Theaters and Cultural halls" } }
};

	

		[ObservableProperty]
		private ObservableCollection<string> availableCategories = new();

		//Properety to store the venue owner
		[ObservableProperty]
        private string ownerName;

        //Properety to store the space name
        [ObservableProperty]
        private string spaceName;

        //Properety to store the City venue
        [ObservableProperty]
        private string city;

        //Properety to sotre the Venue Address 
        //=====>>> You must using Map API to store the Address
        [ObservableProperty]
        private string address;

		//Properety to store the Venue Phone
		[ObservableProperty]
		private string phone;

		//Properety to store the Venue email
		[ObservableProperty]
		private string email;

		//Properety to store the Venue initial price
		[ObservableProperty]
		private string initialPrice;

		//Properety to store the Venue initial price
		[ObservableProperty]
		private double initialPriceValue;

		//Properety to sotre the Cateogry of the Space
		[ObservableProperty]
        private string category;

		//Properety to store the Venue description
		[ObservableProperty]
		private string description;

		//Properety to store the error that appear to the user
		[ObservableProperty]
		private string errorMessage;


		//Properety to store the Venue Capacity
		[ObservableProperty]
         private int capacity;

        //Collection Properety to store the venue images
        [ObservableProperty]
        private ObservableCollection<FileResult> selectedImages;

        //?????
        [ObservableProperty]
        private ObservableCollection<ImageSource> imagePreviewSources;

        //??????
        [ObservableProperty]
        private bool isLoading;

		// Constructor to initialize the collections
		// and set up initial values

		[ObservableProperty]
		private string newServiceName;

		[ObservableProperty]
		private double newServicePrice;		
		
        [ObservableProperty]
		private Guid currentVenueId;

		[ObservableProperty]
		private ObservableCollection<ServiceItem> services;

        //Command to add service for venue
		[RelayCommand]
		private async Task AddService()
		{
			if (string.IsNullOrWhiteSpace(NewServiceName) || NewServicePrice <= 0)
				return;

			var service = new ServiceItem { Name = NewServiceName, Price = NewServicePrice };
			Services.Add(service);

			// Store in Supabase
			await SaveServiceToDatabase(service);

			// Clear inputs
			NewServiceName = string.Empty;
			NewServicePrice = 0;
		}

        //This Method to add the service to the database
		private async Task SaveServiceToDatabase(ServiceItem service)
		{
			try
			{
				// Check if the service exists
				var response = await _supabaseClient
					.From<Service>()
					.Where(s => s.ServiceName == service.Name)
					.Get();
				
				var existingService = response.Models.FirstOrDefault();

				Guid serviceId;

				if (existingService != null)
				{
					serviceId = existingService.ServiceId;
				}
				else
				{
					// Insert new service
					var newService = await _supabaseClient
						.From<Service>()
						.Insert(new Service { ServiceName = service.Name });

					serviceId = newService.Model.ServiceId;

                  //  await Shell.Current.DisplayAlert("ServiceId", serviceId.ToString(), "Ok");
				}

				// Link service to venue
				if (CurrentVenueId == Guid.Empty || serviceId == Guid.Empty)
				{
					await Shell.Current.DisplayAlert("Error", "CurrentVenueId or ServiceId is null", "Ok");
				}
				else
				{
					// Convert price string to appropriate numeric type
					if (service.Price > 0)
					{
						VenueServices venueService = new VenueServices
						{
	//						VenueServiceId = Guid.NewGuid(),  // Generate a new GUID for the Primary key
							VenueId = CurrentVenueId,
							ServiceId = serviceId,
							PricePerUnit = service.Price  // Now using the parsed decimal value
						};
                    
                 //   await Shell.Current.DisplayAlert("info", venueService.VenueId.ToString(), "Ok");

						await _supabaseClient
							.From<VenueServices>()
							.Insert(venueService);
					}
					else
					{
						await Shell.Current.DisplayAlert("Error", "Invalid price format", "Ok");
					}
				}
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Service Error", $"Failed to save service: {ex.Message}", "Ok");
			}
		}

		public AddSpaceViewModel()
        {

			_supabaseClient = SupabaseConfig.SupabaseClient; // Use existing instance
			
            Services = new ObservableCollection<ServiceItem>();

			SelectedSpaceTypes = new ObservableCollection<string>();

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
            // Validate if at least one space type is selected
            if (SelectedSpaceTypes.Count == 0)
            {
                await Shell.Current.DisplayAlert("Validation Error", 
                    "Please select at least one space type.", "OK");
                return;
            }

            // Navigate to SpaceInformationPage
            await Shell.Current.GoToAsync(nameof(SpaceInformationPage));
        }

        // Command to navigate to the SpacePicturesPage
        [RelayCommand]
        private async Task NavigateToSpacePicturesAsync()
        {
            //Check if the Space name is null
            if (String.IsNullOrEmpty(SpaceName)) 
            {
                ErrorMessage = "Please Enter The Space Name";
                return;
            }
            
            //Check if the City is null
            if (String.IsNullOrEmpty(City)) 
            {
                ErrorMessage = "Please Enter The Venue City";
                return;
            }     

            //Check if the Address is null            
            if (String.IsNullOrEmpty(Address)) 
            {
                ErrorMessage = "Please Enter The Venue Address";
                return;
            }
            
            //Check if the Phone is null
            if (String.IsNullOrEmpty(Phone)) 
            {
				ErrorMessage = "Please Enter The Venue Phone";
                return;
            }

            //Check if the Phone number has Correct format
            else
			if (!Regex.IsMatch(Phone, @"^(?:\+962|0)7[789]\d{7}$"))
			{
				ErrorMessage = "Please Enter Valid Phone Number";
				return;
			}

            //Check if the email is null
            if (String.IsNullOrEmpty(Email)) 
            {
                ErrorMessage = "Please Enter Your Venue Email";
                return;
            }

            //Check if the Venue email has correct format
			else 
            if (!Regex.IsMatch(Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z.-]+\.[a-zA-Z]{2,}$"))
			{
				ErrorMessage = "Please Enter Valid email";
				return;
			}

            //Check if the Initial price is null
            if (String.IsNullOrEmpty(InitialPrice)) 
            {
                ErrorMessage = "Please Enter your initial price";
                return;
			}

            else
            {
				InitialPriceValue = Convert.ToDouble(InitialPrice);
			}

            //Check if the User Enter Valid initial price
            if (InitialPriceValue < 0) 
            {
                ErrorMessage = "Please Enter a valid initial price";
                return;
            }

            //Check if the Category is null
            if (String.IsNullOrEmpty(Category)) 
            {
                ErrorMessage = "Please Enter Your Space Category";
                return;
            }

            //Check if the Description is null
            if (String.IsNullOrEmpty(Description)) 
            {
                ErrorMessage = "Please Enter Description for Your Venue";
                return;
            }

			await Shell.Current.GoToAsync(nameof(SpacePicturesPage));
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
            // Collect non-null images
            var imagesToUpload = SelectedImages
                                .Where(img => img != null).ToList();

            var venue = new Venue
            {
                // No VenueId - let Supabase generate it
                OwnerId = Guid.Parse(SupabaseConfig.SupabaseClient.Auth.CurrentUser.Id),
                VenueName = SpaceName,
                Description = Description,
                Type = SpaceType,
                Location = $"{City}, {Address}",
                VenueContactPhone = Phone,
                VenueEmail = Email,
                InitialPrice = InitialPriceValue,
                Status = "Pending"
            };

            var result = await VenueService
                    .CreateVenueAsync(venue, imagesToUpload,
                    Category, Description);

            if (result.Success && result.VenueId.HasValue)
            {
                // Set the CurrentVenueId property with the ID from the database
                CurrentVenueId = result.VenueId.Value;
            //    await Shell.Current.DisplayAlert("Success", $"Venue created with ID: {CurrentVenueId}", "OK");
                
                // Navigate to ServicesPage
                await Shell.Current.GoToAsync(nameof(ServicesPage));
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to create venue", "OK");
            }
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
                string.IsNullOrWhiteSpace(Category) ||
                string.IsNullOrWhiteSpace(City) ||
                string.IsNullOrWhiteSpace(Address))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Please fill in all required fields.", "OK");
                return;
            }

            // Collect non-null images
            var imagesToUpload = SelectedImages
                                .Where(img => img != null).ToList();

            if (!imagesToUpload.Any())
            {
                await Shell.Current.DisplayAlert("Warning",
                    "Please select at least one image.", "OK");
                return;
            }

            //try
            //{
                //IsLoading = true;
                //await Shell.Current.DisplayAlert("Processing",
                //      "Saving your venue and uploading images.
                //      Please wait...", "OK");

                // Create a new venue object
                //    var venue = new Venue
                //    {
                //        VenueId = Guid.NewGuid(),
                //        OwnerId = Guid.Parse(SupabaseConfig.SupabaseClient.Auth.CurrentUser.Id),
                //        VenueName = SpaceName,
                //        Description = Description,
                //        Type = SpaceType, // This will contain all selected types as a comma-separated string
                //    //    Capacity = Capacity, // the number may be stored wrong in the database
                //        Location = $"{City}, {Address}",//Find way to store it by map
                //        VenueContactPhone = Phone, // Example value
                //        VenueEmail = Email, // Example value
                //        InitialPrice = InitialPriceValue, // the number may be stored wrong in the database
                //        Status = "Pending" // Set status to pending
                //    };

                //    //Console.WriteLine($"Creating venue with ID: {venue.VenueId}");

                //    // Save venue with uploaded images and category
                //    bool success = await VenueService
                //                        .CreateVenueAsync(venue, imagesToUpload,
                //                        Category, Description);

                //    if (success)
                //    {
                //        await Shell.Current.DisplayAlert("Success", 
                //            "Venue created successfully and is pending approval!", "OK");

                //        await Shell.Current.GoToAsync(nameof(SuccessPage));
                //    }

                //    else
                //    {
                //        await Shell.Current.DisplayAlert("Error", 
                //            "Failed to create venue.", "OK");
                //    }
                //}
                //catch (Exception ex)
                //{
                //    //Console.WriteLine($"Error saving venue: {ex.Message}");
                //    await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                //}
                //finally
                //{
                //    IsLoading = false;
                //}
            
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
                    if (int.TryParse(index, out int idx) && 
                        idx >= 0 && idx < SelectedImages.Count)
                    {
                        SelectedImages[idx] = result;

                        // Create an image preview
                        var stream = await result.OpenReadAsync();
                        ImagePreviewSources[idx] = ImageSource.FromStream
                                                                (() => stream);
                        OnPropertyChanged(nameof(ImagePreviewSources));
                    }

                    else
                    {
                        await Shell.Current.DisplayAlert("Error", 
                            "Invalid image index.", "OK");
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

		#region Space Type Commands
		//Method to update the Availiable Categories for every space type
        private void UpdateAvailableCategories()
		{
			var selectedCategories = new HashSet<string>();

			foreach (var spaceType in SelectedSpaceTypes)
			{
				if (SpaceTypeCategories.TryGetValue(spaceType, out var categories))
				{
					foreach (var category in categories)
					{
						selectedCategories.Add(category);
					}
				}
			}

			AvailableCategories = new ObservableCollection<string>(selectedCategories);
		}

        //Method help the user to Choose more than Space type
		[RelayCommand]
		private async Task ToggleSpaceType(string spaceTypeValue)
		{
			if (spaceTypeValue == null)
				return;

			// Define categories
			var exclusiveTypes = new HashSet<string> { "Funeral", "Photography", "Sports" };
			var groupTypes = new HashSet<string> { "Wedding", "Work/Meeting Space", "Cultural Events", "Entertainment" };

			if (exclusiveTypes.Contains(spaceTypeValue))
			{
				// If an exclusive type is selected but the user has already selected a group type, show an alert
				if (SelectedSpaceTypes.Any(type => groupTypes.Contains(type)))
				{
					await Shell.Current.DisplayAlert(
						"Invalid Selection",
						"You cannot select 'Funeral', 'Photography', or 'Sports' with other types.",
						"OK"
					);
					return; // Stop execution to prevent invalid selection
				}

				// If the type is already selected, remove it from the list (toggle it)
				if (SelectedSpaceTypes.Contains(spaceTypeValue))
				{
					SelectedSpaceTypes.Remove(spaceTypeValue);
				}
				else
				{
					// Otherwise, clear previous selections and add the exclusive type
					SelectedSpaceTypes.Clear();
					SelectedSpaceTypes.Add(spaceTypeValue);
				}
			}
			else if (groupTypes.Contains(spaceTypeValue))
			{
				// If the user has already selected an exclusive type, show an alert
				if (SelectedSpaceTypes.Any(type => exclusiveTypes.Contains(type)))
				{
					await Shell.Current.DisplayAlert(
						"Invalid Selection",
						"You cannot select 'Weeding', 'Meeting', 'Cultural Events', or 'Entertainment' with 'Funeral', 'Photography', or 'Sports'.",
						"OK"
					);
					return; // Stop execution to prevent invalid selection
				}

				// If the group type is already selected, remove it (toggle it)
				if (SelectedSpaceTypes.Contains(spaceTypeValue))
				{
					SelectedSpaceTypes.Remove(spaceTypeValue);
				}
				else
				{
					// Otherwise, add the group type to the list
					SelectedSpaceTypes.Add(spaceTypeValue);
				}
			}

			// Update the spaceType string property for database storage
			SpaceType = string.Join(", ", SelectedSpaceTypes);

			UpdateAvailableCategories();

			// Notify the UI to reflect changes without recreating the ObservableCollection
			OnPropertyChanged(nameof(SelectedSpaceTypes));
		}

        //Method to check if the user select this space type or not 
		public bool IsSpaceTypeSelected(string spaceTypeValue)
        {
            return SelectedSpaceTypes.Contains(spaceTypeValue);
        }

        #endregion
    }
}