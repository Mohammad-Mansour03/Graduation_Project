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
using CommunityToolkit.Mvvm.Messaging;
using HojozatyCode.Messages;

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

			{ "Photography", new List<string> { "Photography Studios", "Outdoor Photography Spaces" ,
				"Product Photography Spaces" } },

			{ "Sports", new List<string> { "Staduim" } },

			{ "Cultural Events", new List<string> { "Farms", "Majls", "Cultrual Evening Venues" , "Theaters and Cultural halls" } }
		};


		//Collection to store what is the Categiries was available to show depend on the space type
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

		[ObservableProperty]
		private double latitude;

		[ObservableProperty]
		private double longitude;

		[ObservableProperty]
		private string selectedLocation;

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
		private string capacityInput;
		
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

		[ObservableProperty]
		private List<string> imageUrlsList;


		//Service Page

		//store the service name
		[ObservableProperty]
		private string newServiceName;

		//store the price for the service
		[ObservableProperty]
		private double newServicePrice;

		//store the current venueId		
		[ObservableProperty]
		private Guid currentVenueId;

		//Collection to sotre the list of services the user add
		[ObservableProperty]
		private ObservableCollection<ServiceItem> services;

		//This generates the SelectedCancellationPolicy property correctly
		[ObservableProperty]
		private string selectedPolicy;		
		
		//To Store the House Rule
		[ObservableProperty]
		private string newHouseRuleName;		
		
		//To Store The description for the House Rule
		[ObservableProperty]
		private string newHouseRuleDescription;

		//Collection to sotre the list of house rules the user add
		[ObservableProperty]
		private ObservableCollection<HouseRule> houseRules;



		//The Constructor
		public AddSpaceViewModel()
		{

			_supabaseClient = SupabaseConfig.SupabaseClient; // Use existing instance

			Services = new ObservableCollection<ServiceItem>();

			HouseRules = new ObservableCollection<HouseRule>();

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
			// if (String.IsNullOrEmpty(Address))
			// {
			// 	ErrorMessage = "Please Enter The Venue Address";
			// 	return;
			// }

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

	

			//Check if the capacity is null
			if (String.IsNullOrEmpty(CapacityInput))
			{
				ErrorMessage = "Please Enter your initial price";
				return;
			}
			else
			{
				Capacity = Convert.ToInt32(CapacityInput);
			}
			//Check if the User Enter Valid initial price
			if (Capacity < 0)
			{
				ErrorMessage = "Please Enter a valid Capacity";
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
				Location = $"{Latitude}, {Longitude}",
				City = City,
				VenueContactPhone = Phone,
				VenueEmail = Email,
				Capacity = Capacity,
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


		// Command to navigate to the SpacePoliciesPage
		[RelayCommand]
		private async Task NavigateToSpacePoliciesPageAsync() =>
			await Shell.Current.GoToAsync(nameof(SpacePolicies));

		// Command to navigate to the ReviewPage
		[RelayCommand]
		private async Task NavigateToReviewPageAsync()
		{
			// Ensure we have the cancellation policy updated
			await UpdateCancellationPolicyAsync(CurrentVenueId, SelectedPolicy);
			
			// Parse the venue's ImageUrl into a list of image URLs
			if (CurrentVenueId != Guid.Empty)
			{
				try
				{
					// Fetch the venue to get its ImageUrl
					var venueResponse = await _supabaseClient
						.From<Venue>()
						.Where(v => v.VenueId == CurrentVenueId)
						.Get();
						
					if (venueResponse != null && venueResponse.Models.Count > 0)
					{
						var venueImageUrl = venueResponse.Models[0].ImageUrl;
						if (!string.IsNullOrEmpty(venueImageUrl))
						{
							// Split by comma and remove empty entries
							ImageUrlsList = venueImageUrl
								.Split(',')
								.Where(url => !string.IsNullOrWhiteSpace(url))
								.ToList();
						}
						else
						{
							ImageUrlsList = new List<string>();
						}
					}
				}
				catch (Exception ex)
				{
					await Shell.Current.DisplayAlert("Error", $"Failed to load venue images: {ex.Message}", "OK");
					ImageUrlsList = new List<string>();
				}
			}
			
			await Shell.Current.GoToAsync(nameof(ReviewPage));
		}


		// Command to navigate to the SuccessPage
		[RelayCommand]
		private async Task NavigateToSuccessPageAsync() =>
			await Shell.Current.GoToAsync(nameof(SuccessPage));

		// Command to navigate to the HomePage
		[RelayCommand]
		private async Task GoToHome() =>
			await Shell.Current.GoToAsync(nameof(HomePage));
		
		// Command to navigate to the HomePage
		[RelayCommand]
		private async Task GoToMySpace() =>
			await Shell.Current.GoToAsync(nameof(Pages.MySpace));		
		



		#endregion

		#region Venue Save & Image Commands

		// Command to navigate to the Success Page
		[RelayCommand]
		private async Task SaveSpaceAsync()
		{
			await Shell.Current.GoToAsync(nameof(Pages.SuccessPage));
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

		#region Service Page Commands
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
							VenueId = CurrentVenueId,
							ServiceId = serviceId,
							PricePerUnit = service.Price  // Now using the parsed decimal value
						};

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


		[RelayCommand]
		private async Task DeleteService(ServiceItem service)
		{
			if (service == null)
			{
				await Shell.Current.DisplayAlert("Error", "Service is null", "OK");
				return;
			}
			try
			{
				var serviceRecord = await _supabaseClient
							   .From<Service>()
							   .Where(s => s.ServiceName == service.Name)
							   .Single();

			
				if (serviceRecord == null)
				{
					await Shell.Current.DisplayAlert("Error", "Service not found in database", "OK");
					return;
				}


				// Step 2: Find the corresponding VenueService entry
				var venueService = await _supabaseClient
					.From<VenueServices>()
					.Where(vs => vs.ServiceId == serviceRecord.ServiceId) // Match using found ServiceId
					.Single();

				if (venueService == null)
				{
					await Shell.Current.DisplayAlert("Error", "Venue Service not found", "OK");
					return;
				}

				// Delete the record from the VenueServices table
				var response = _supabaseClient
					.From<VenueServices>()
					.Where(x => x.ServiceId == venueService.ServiceId && x.VenueId == venueService.VenueId)
					.Delete();

				if (response != null)
				{
					Services.Remove(service);
					await Shell.Current.DisplayAlert("Done", "Service deleted successfully.", "OK");
				}
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", ex.Message, "OK");


				//}
				//await Shell.Current.GoToAsync(nameof(LogInPage));
			}
		}

		#endregion

		#region Space Polices Commands


		private async Task UpdateCancellationPolicyAsync(Guid venueId, string cancellationPolicy)
		{
			if (venueId == Guid.Empty || string.IsNullOrEmpty(cancellationPolicy))
				return;

			try
			{
				//await Shell.Current.DisplayAlert("Canceelation Policy", cancellationPolicy, "OK");

				var response = await _supabaseClient
					.From<Venue>()
					.Where(v => v.VenueId == venueId)  // Ensure VenueId is the PK
					.Set(v => v.CancellationPolicy, cancellationPolicy)  // Use .Set() for property update
					.Update();


				if (response != null && response.Models.Count > 0)
				{
					await Shell.Current.DisplayAlert("Done","Cancellation policy updated successfully.", "Ok");
				}

				else
				{
					await Shell.Current.DisplayAlert("Error", "Error to update Cancellation policy", "Ok");
				}
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
			}
		}

		//Command to add House Rule for venue
		[RelayCommand]
		private async Task AddHouseRule()
		{
			if (string.IsNullOrWhiteSpace(NewHouseRuleName) || string.IsNullOrWhiteSpace(NewHouseRuleDescription))
				return;
			try
			{
				HouseRule houseRule = new HouseRule { HouseRuleName = NewHouseRuleName, HouseRuleDescription = NewHouseRuleDescription };
				HouseRules.Add(houseRule);

				// Store in Supabase
				await SaveHouseRuleToDatabase(houseRule);

				// Clear inputs
				NewHouseRuleName = string.Empty;
				NewHouseRuleDescription = string.Empty;
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
			}
		}

		private async Task SaveHouseRuleToDatabase(HouseRule hostRule)
		{
			try
			{
				// Check if the service exists
				var response = await _supabaseClient
					.From<HostRules>()
					.Where(s => s.HostRuleName == hostRule.HouseRuleName)
					.Get();

				var existingHostRule = response.Models.FirstOrDefault();

				Guid hostRuleId;

				if (existingHostRule != null)
				{
					hostRuleId = existingHostRule.HostRuleId;
				}
				else
				{
					// Insert new host rule
					var newService = await _supabaseClient
						.From<HostRules>()
						.Insert(new HostRules { HostRuleName = hostRule.HouseRuleName ,
												HostRuleDescription = hostRule.HouseRuleDescription});

					hostRuleId = newService.Model.HostRuleId;

				}

				// Link host rule to venue
				if (CurrentVenueId == Guid.Empty || hostRuleId == Guid.Empty)
				{
					await Shell.Current.DisplayAlert("Error", "CurrentVenueId or HostRule is null", "Ok");
				}
				else
				{
					HostRulesVenues hostRuleVenue = new HostRulesVenues
					{
							VenueId = CurrentVenueId,
							HostRuleId = hostRuleId
					};

						await _supabaseClient
							.From<HostRulesVenues>()
							.Insert(hostRuleVenue);
				}
					
				
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Host Rule Error", $"Failed to save host rule: {ex.Message}", "Ok");
			}
		}

		[RelayCommand]
		private async Task DeleteHostRule(HouseRule hostRule)
		{
			if (hostRule == null)
			{
				await Shell.Current.DisplayAlert("Error", "Host Rule is null", "OK");
				return;
			}
			try
			{
				var hostRuleRecord = await _supabaseClient
							   .From<HostRules>()
							   .Where(s => s.HostRuleName == hostRule.HouseRuleName)
							   .Single();


				if (hostRuleRecord == null)
				{
					await Shell.Current.DisplayAlert("Error", "Host Rule not found in database", "OK");
					return;
				}


				// Step 2: Find the corresponding VenueService entry
				var hostRuleVenue = await _supabaseClient
					.From<HostRulesVenues>()
					.Where(vs => vs.HostRuleId == hostRuleRecord.HostRuleId) // Match using found ServiceId
					.Single();

				if (hostRuleVenue == null)
				{
					await Shell.Current.DisplayAlert("Error", "Host Rule Venue not found", "OK");
					return;
				}

				// Delete the record from the VenueServices table
				var response = _supabaseClient
					.From<HostRulesVenues>()
					.Where(x => x.HostRuleId == hostRuleRecord.HostRuleId && x.VenueId == hostRuleVenue.VenueId)
					.Delete();

				if (response != null)
				{
					HouseRules.Remove(hostRule);
					await Shell.Current.DisplayAlert("Done", "Host Rule deleted successfully.", "OK");
				}
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", ex.Message, "OK");

			}
		}
		#endregion

		#region The Map Location Code
		[RelayCommand]
		private async Task SearchLocation(string searchQuery)
		{
			if (string.IsNullOrWhiteSpace(searchQuery))
				return;
			
			ErrorMessage = string.Empty; // Clear any previous errors
			
			try {
				// Show loading indicator
				IsLoading = true;
				
				var locations = await Geocoding.GetLocationsAsync(searchQuery);
				var location = locations?.FirstOrDefault();
				
				if (location != null)
				{
					// Update ViewModel properties
					Latitude = location.Latitude;
					Longitude = location.Longitude;
					SelectedLocation = $"Found: {searchQuery}\nLat: {location.Latitude}, Lng: {location.Longitude}";
					
					// Notify the page to update the map
					WeakReferenceMessenger.Default.Send(new LocationFoundMessage(location));
				}
				else
				{
					ErrorMessage = $"No locations found for '{searchQuery}'";
				}
			}
			catch (Exception ex) {
				ErrorMessage = $"Search failed: {ex.Message}";
			}
			finally {
				IsLoading = false;
			}
		}
		#endregion
	}
}

