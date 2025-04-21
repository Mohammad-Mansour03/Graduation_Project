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

		// التصنيف الحالي للعرض
		public string SelectedCategory { get; set; }


		//Properety to store the sub categories
		[ObservableProperty]
        private List<Subcategory> subCategories = new();

        //Properety to store the Main Category
        [ObservableProperty]
        private string category;

		public ObservableCollection<CitieisEnum> Cities { get; set; } = new ObservableCollection<CitieisEnum>((CitieisEnum[])Enum.
            GetValues(typeof(CitieisEnum)));

		[ObservableProperty]
		private CitieisEnum selectedCity;

		//Properety to store list of Venues related to that categroy
		[ObservableProperty]
        private ObservableCollection<Venue> venues = new();
         


        //Method apply when the user choose category
        partial void OnCategoryChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Task.Run(async () => await LoadVenuesForCategory(value));
            }
        }

        //Method to load the Correctv subcategories and the correct venues
        private async Task LoadVenuesForCategory(string categoryName)
        {
            SelectedCategory = categoryName;

            //Store the all subcategories with their related photos
            var spaceCategories = new Dictionary<string, List<Subcategory>>
            {
                { "Wedding", new List<Subcategory>
                    {
                        new Subcategory { Name = "Halls", ImageUrl = "halls.png" },
                        new Subcategory { Name = "Farms", ImageUrl = "farms.jpg" },
                        new Subcategory { Name = "Hotels", ImageUrl = "hotel1.jpg" },
                        new Subcategory { Name = "Outdoors", ImageUrl = "outdoors.jpg" }
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
                { "Sport", new List<Subcategory>
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
                var venuesResult = await client
                    .From<Venue>()
                    .Where(v => v.Type == categoryName)
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
            if (!string.IsNullOrEmpty(Category))
			{
				await Shell.Current.GoToAsync($"{nameof(Pages.FiltersPage)}?category={Category}");
			}
		}


        // أمر الفلترة (يتم استدعاؤه لما ترجع البيانات من صفحة الفلتر)
        [RelayCommand]
        public async Task ApplyFilterAsync(VenueFilter filter)
        {
            await Shell.Current.DisplayAlert("Prompt", "Inside the Apply filter in Category venue view model", "OK");
            // استعلام لجميع الأماكن حسب التصنيف
            var response = await SupabaseConfig.SupabaseClient
                .From<Venue>()
                .Where(v => v.Type == SelectedCategory)
                .Get();

            var filtered = response.Models;


            await Shell.Current.DisplayAlert("The Number of venues inside the response before filtered", $"{filtered.Count()}", "OK");


            foreach (var venue in response.Models) 
            {
				await Shell.Current.DisplayAlert("Number of attendesses and price ", $"VenueCpacity{venue.Capacity}\nVenuePrice{venue.InitialPrice}", "OK");
			}


			await Shell.Current.DisplayAlert("Prompt", $"MinPrice{filter.MinPrice}\nMaxPrice{filter.MaxPrice}\nMinCapacity{filter.MinCapacity}\nMaxCapacity{filter.MaxCapacity}", "OK");

			// فلترة محلية للبيانات بناءً على الفلتر القادم
			 filtered = response.Models
                .Where(v => (v.InitialPrice >= filter.MinPrice && v.InitialPrice <= filter.MaxPrice)
                    && (v.Capacity >= filter.MinCapacity && v.Capacity <= filter.MaxCapacity)
                   )
                .ToList();

            
            await Shell.Current.DisplayAlert("Number of venues before filtered", $"{Venues.Count()}", "OK");
            // تحديث الواجهة بالنتائج
            Venues.Clear();

            foreach (var venue in filtered)
            {
                Venues.Add(venue);
            }

			await Shell.Current.DisplayAlert("Number of venues after filtered", $"{Venues.Count()}", "OK");


			foreach (var venue in Venues)
			{
				await Shell.Current.DisplayAlert("Number of attendesses and price ", $"VenueCpacity{venue.Capacity}\nVenuePrice{venue.InitialPrice}", "OK");
			}

		}

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
