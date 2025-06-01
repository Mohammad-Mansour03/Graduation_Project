using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using System;
using System.Collections.ObjectModel;

namespace HojozatyCode.ViewModels
{
    [QueryProperty("Category", "Category")]
    public partial class CategoryVenuesViewModel : ObservableObject
    {


		// The Current Category
		public string SelectedCategory { get; set; }

		//Properety to store the sub categories
		[ObservableProperty]
        private List<Subcategory> subCategories = new();

        //Properety to store the Main Category
        [ObservableProperty]
        private string category;

        //Collection to store the all cities
		public ObservableCollection<CitieisEnum> Cities { get; set; } =
            new ObservableCollection<CitieisEnum>((CitieisEnum[])Enum.
            GetValues(typeof(CitieisEnum)));
        
        private bool isFiltered = false;

        //Store the City that the user choose
		[ObservableProperty]
		private CitieisEnum selectedCity;

		//Properety to store list of Venues related to that categroy that will display inside the Category Venue Page
		[ObservableProperty]
        private ObservableCollection<Venue> venues = new();


        //Method apply when the user choose category
        partial void OnCategoryChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                LoadVenuesForCategory(value).ConfigureAwait(false);
            }
        }

        //Method to load the Correct subcategories and the correct venues
        private async Task LoadVenuesForCategory(string categoryName)
        {
            if (isFiltered) return;

            SelectedCategory = categoryName;

            //Store the all subcategories with their related photos
            var spaceCategories = new Dictionary<string, List<Subcategory>>
            {
                { "Wedding", new List<Subcategory>
                    {
                        new Subcategory { Name = "Halls", ImageUrl = "halls.png" },
                        new Subcategory { Name = "Farms", ImageUrl = "farms.jpg" },
                        new Subcategory { Name = "Hotels", ImageUrl = "hotel1.jpg" },
                        new Subcategory { Name = "Outdoor Space", ImageUrl = "outdoors.jpg" }
                    }
                },
                { "Entertainment", new List<Subcategory>
                    {
                        new Subcategory { Name = "Farms", ImageUrl = "farms.jpg" },
                        new Subcategory { Name = "Adventure Spots", ImageUrl = "adventure.png" },
                        new Subcategory { Name = "WorkShops", ImageUrl = "workshop.png" }
                    }
                },
                { "Meeting", new List<Subcategory>
                    {
                        new Subcategory { Name = "ClassRooms/Office Spaces", ImageUrl = "office.png" },
                        new Subcategory { Name = "Farms", ImageUrl = "farms.jpg" },
                        new Subcategory { Name = "Outdoor Space", ImageUrl = "outdoors.jpg" },
                        new Subcategory { Name = "Majls", ImageUrl = "majls.png" }
                    }
                },
                { "Funeral", new List<Subcategory>
                    {
                        new Subcategory { Name = "Diwan", ImageUrl = "diwan.png" },
                        new Subcategory { Name = "Dedicated Funeral Halls", ImageUrl = "funeral.png" }
                    }
                },
                { "Photography", new List<Subcategory>
                    {
                        new Subcategory { Name = "Photography Studios", ImageUrl = "photo.png" },
                        new Subcategory { Name = "Outdoor Photography Spaces", ImageUrl = "photoout.png" },
                        new Subcategory { Name = "Product Photography Spaces", ImageUrl = "product.png" }
                    }
                },
                { "Sports", new List<Subcategory>
                    {
                        new Subcategory { Name = "Stadium", ImageUrl = "stadium.png" }
                    }
                },
                { "Cultural Events", new List<Subcategory>
                    {
                        new Subcategory { Name = "Farms", ImageUrl = "farms.jpg" },
                        new Subcategory { Name = "Majls", ImageUrl = "majls.png" },
                        new Subcategory { Name = "Cultural Evening Venues", ImageUrl = "cultural.png" },
                        new Subcategory { Name = "Theaters and Cultural Halls", ImageUrl = "theater.png" }
                    }
                }
            };

            try
            {
                //Store the Correct sub categories
                SubCategories = spaceCategories.ContainsKey(categoryName) ? spaceCategories[categoryName] : new List<Subcategory>();

                //Returen the all venues related to this category
                var client = SupabaseConfig.SupabaseClient;

                //Convert the City from Enum to String
                var cityConvert = Enum.GetName(typeof(CitieisEnum) , SelectedCity);

                //Return the Venue Result depending on the Venue must Approved , City , and it's Category
				var venuesResult = await client
					.From<Venue>()
					.Filter("status", Supabase.Postgrest.Constants.Operator.Equals, "Approved")
					.Filter("city", Supabase.Postgrest.Constants.Operator.Equals, cityConvert)
					.Filter("type", Supabase.Postgrest.Constants.Operator.ILike, $"%{categoryName}%")
					.Get();

				Venues.Clear();


                //Store the all venues return from supabase to store it inside the Venue Observable properety
                foreach (var venue in venuesResult.Models)
                {
                    Venues.Add(venue);
                }
            }

            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load venues: {ex.Message}", "OK");
            }
        }

		//Navigation Command to navigate me to the filter page
		[RelayCommand]
		private async Task GoToFiltersPage()
		{
            if (!string.IsNullOrEmpty(Category) )
			{
				await Shell.Current.GoToAsync($"{nameof(Pages.FiltersPage)}?category={Category}");
			}
		}


        //Fetching the new venues depending on the filtering data
        [RelayCommand]
        public async Task ApplyFilterAsync(VenueFilter filter)
        {

            isFiltered = true;

			var cityConvert = Enum.GetName(typeof(CitieisEnum), filter.FilterCity);

		

            // Reture the all venues that related to the category
			var response = await SupabaseConfig.SupabaseClient
                    .From<Venue>()
					.Filter("status", Supabase.Postgrest.Constants.Operator.Equals, "Approved")
					.Filter("city", Supabase.Postgrest.Constants.Operator.Equals, cityConvert)
					.Filter("type", Supabase.Postgrest.Constants.Operator.ILike, $"%{SelectedCategory}%")
					.Get();

            var filtered = response.Models;
            
            // Filtering the Venues depending on your data 
			 filtered = response.Models				
	                    .Where(v =>
		                    (filter.MinPrice == 0 || v.InitialPrice >= filter.MinPrice) &&
		                    (filter.MaxPrice == 0 || v.InitialPrice <= filter.MaxPrice) &&
		                    (filter.MinCapacity == 0 || v.Capacity >= filter.MinCapacity) &&
		                    (filter.MaxCapacity == 0 || v.Capacity <= filter.MaxCapacity)
	                    )
	                    .ToList();

           	Venues.Clear();

            foreach (var venue in filtered)
            {
                Venues.Add(venue);
            }

		}

        //Method to deal with the Selected City Changing
		partial void OnSelectedCityChanged(CitieisEnum value)
		{
            if (!string.IsNullOrEmpty(SelectedCategory))
				MainThread.BeginInvokeOnMainThread(async () => await LoadVenuesForCategory(SelectedCategory));
		}

        //Command to navigate me to the Venues Booking Process 
		[RelayCommand]
        public async Task VenueSelectedAsync(Venue selectedVenue) 
        {
			if (selectedVenue == null) return;

			var navParams = new Dictionary<string, object>
		    {
			    { "SelectedVenue", selectedVenue }
		    };
			await Shell.Current.GoToAsync(nameof(Pages.ChoosingHallBooking) , navParams);
        }
	}

}
